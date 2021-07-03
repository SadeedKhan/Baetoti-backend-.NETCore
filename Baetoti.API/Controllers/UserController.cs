using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.API.Helpers;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Request.User;
using Baetoti.Shared.Response.FileUpload;
using Baetoti.Shared.Response.Shared;
using Baetoti.Shared.Response.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class UserController : ApiBaseController
    {
        public readonly IUserRepository _userRepository;
        public readonly IOTPRepository _otpRepository;
        public readonly IProviderRepository _providerRepository;
        public readonly IDriverRepository _driverRepository;
        public readonly IMapper _mapper;

        public UserController(
            IUserRepository userRepository,
            IOTPRepository otpRepository,
            IProviderRepository providerRepository,
            IDriverRepository driverRepository,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _otpRepository = otpRepository;
            _providerRepository = providerRepository;
            _driverRepository = driverRepository;
            _mapper = mapper;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignUpRequest signUpRequest)
        {
            try
            {
                var existingUser = await _userRepository.GetByMobileNumberAsync(signUpRequest.MobileNumber);
                if (existingUser == null)
                {
                    var user = new User();
                    user.Phone = signUpRequest.MobileNumber;
                    user.UserStatus = (int)UserStatus.Active;
                    existingUser = await _userRepository.AddAsync(user);
                    if (existingUser == null)
                        return Ok(new SharedResponse(false, 400, "Unable to SignUp. Please try later"));
                }
                var _min = 1000;
                var _max = 9999;
                var _rdm = new Random();
                var number = _rdm.Next(_min, _max);
                var otp = new OTP();
                otp.UserID = existingUser.ID;
                otp.OneTimePassword = number.ToString();
                otp.OTPGeneratedAt = DateTime.Now;
                otp.OTPStatus = (int)OTPStatus.Active;
                otp.RetryCount = 0;
                var insertedOTP = await _otpRepository.AddAsync(otp);
                return Ok(new SharedResponse(true, 200, "User Signed Up Successfully", otp));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP([FromBody] OTPRequest otpRequest)
        {
            try
            {
                var varifiedOTP = await _otpRepository.GetByUserIdAsync(otpRequest.UserID);
                if (varifiedOTP == null)
                    return Ok(new SharedResponse(false, 400, "Unable to Verify OTP. Please Try Later."));
                if (varifiedOTP.OTPGeneratedAt < DateTime.Now.AddMinutes(-2))
                {
                    varifiedOTP.OTPStatus = (int)OTPStatus.Rejected;
                    varifiedOTP.Description = "Rejected Due to Timeout.";
                    await _otpRepository.UpdateAsync(varifiedOTP);
                    return Ok(new SharedResponse(false, 400, "OTP Expire. Please Try Later."));
                }
                varifiedOTP.OTPStatus = (int)OTPStatus.Approved;
                varifiedOTP.Description = "Approved";
                await _otpRepository.UpdateAsync(varifiedOTP);
                var user = await _userRepository.GetByIdAsync(otpRequest.UserID);
                user.LastLogin = DateTime.Now;
                await _userRepository.UpdateAsync(user);
                return Ok(new SharedResponse(true, 200, "Login Successfully", user));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("CompleteProfile")]
        public async Task<IActionResult> CompleteProfile([FromBody] CompleteProfileRequest completeProfileRequest)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(completeProfileRequest.UserID);
                if (user == null)
                    return Ok(new SharedResponse(false, 400, "Unable to Find User Information. Please Try Later."));
                user.FirstName = completeProfileRequest.FirstName;
                user.LastName = completeProfileRequest.LastName;
                user.DOB = completeProfileRequest.DOB;
                user.Gender = completeProfileRequest.Gender;
                user.Email = completeProfileRequest.Email;
                user.UpdatedBy = Convert.ToInt32(UserId);
                user.LastUpdatedAt = DateTime.Now;
                user.IsProfileCompleted = true;
                user.Picture = completeProfileRequest.Picture;
                await _userRepository.UpdateAsync(user);
                return Ok(new SharedResponse(true, 200, "Profile Updated Successfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    UploadImage obj = new UploadImage();
                    FileUploadResponse _RESPONSE = await obj.UploadImageFile(file, "User");
                    if (string.IsNullOrEmpty(_RESPONSE.Message))
                    {
                        return Ok(new SharedResponse(true, 200, "File uploaded successfully!", _RESPONSE.Path, _RESPONSE.FileName, _RESPONSE.PathwithFileName));
                    }
                    else
                    {
                        return Ok(new SharedResponse(true, 400, _RESPONSE.Message));
                    }
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "File is required!"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userList = (await _userRepository.ListAllAsync()).Select(x => new UserList
                {
                    UserID = x.ID,
                    Name = $"{x.FirstName} {x.LastName}",
                    Revenue = "0",
                    MobileNumber = x.Phone,
                    Address = x.Address,
                    SignUpDate = x.CreatedAt,
                    UserStatus = x.UserStatus == 1 ? "Active" : "Inactive",
                    ProviderStatus = "-",
                    DriverStatus = "-"
                }).ToList();
                var userResponse = new UserResponse();
                userResponse.userList = userList;
                var userSammary = new UserSummary();
                userSammary.TotalUser = userList.Count();
                userSammary.ActiveUser = userList.Where(x => x.UserStatus == "Active").Count();
                userSammary.NewUser = userList.Where(x => x.SignUpDate >= DateTime.Now.AddMonths(-1)).Count();
                userSammary.LiveUser = 0;
                userResponse.userSummary = userSammary;
                userResponse.providerSummary = new ProviderSummary();
                userResponse.driverSummary = new DriverSummary();
                return Ok(new SharedResponse(true, 200, "", userResponse));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

    }
}
