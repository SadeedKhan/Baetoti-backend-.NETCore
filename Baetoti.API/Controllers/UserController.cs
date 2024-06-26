﻿using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.API.Helpers;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Request.Commission;
using Baetoti.Shared.Request.User;
using Baetoti.Shared.Request.VAT;
using Baetoti.Shared.Response.Commission;
using Baetoti.Shared.Response.FileUpload;
using Baetoti.Shared.Response.Invoice;
using Baetoti.Shared.Response.Shared;
using Baetoti.Shared.Response.VAT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public readonly ICommissionRepository _commissionRepository;
        public readonly IVATRepository _vatRepository;
        public readonly IMapper _mapper;

        public UserController(
            IUserRepository userRepository,
            IOTPRepository otpRepository,
            IProviderRepository providerRepository,
            IDriverRepository driverRepository,
            ICommissionRepository commissionRepository,
            IVATRepository vatRepository,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _otpRepository = otpRepository;
            _providerRepository = providerRepository;
            _driverRepository = driverRepository;
            _commissionRepository = commissionRepository;
            _vatRepository = vatRepository;
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
                if (varifiedOTP.OneTimePassword != otpRequest.OTP)
                    return Ok(new SharedResponse(false, 400, "Invalid OTP. Please Try Again."));
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
                        return Ok(new SharedResponse(true, 200, "File uploaded successfully!", _RESPONSE));
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
                var usersData = await _userRepository.GetAllUsersDataAsync();
                return Ok(new SharedResponse(true, 200, "", usersData));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("ViewProfile")]
        public async Task<IActionResult> ViewProfile(long UserID)
        {
            try
            {
                var userData = await _userRepository.GetUserProfile(UserID);
                return Ok(new SharedResponse(true, 200, "", userData));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("GetFilteredData")]
        public async Task<IActionResult> GetFilteredData([FromBody] UserFilterRequest filterRequest)
        {
            try
            {
                var usersData = await _userRepository.GetFilteredUsersDataAsync(filterRequest);
                return Ok(new SharedResponse(true, 200, "", usersData));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetAllOnBoardingRequest")]
        public async Task<IActionResult> GetAllOnBoardingRequest()
        {
            try
            {
                var onBoardingResponse = await _userRepository.GetOnBoardingDataAsync();
                return Ok(new SharedResponse(true, 200, "", onBoardingResponse));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }


        //Commissions
        [HttpGet("GetAllCommissions")]
        public async Task<IActionResult> GetAllCommissions()
        {
            try
            {
                var commissionList = (await _commissionRepository.ListAllAsync()).
                    Where(x => x.MarkAsDeleted == false && x.UserTypeID == (int)UserType.Buyer).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<CommissionResponse>>(commissionList)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetCommissionsById")]
        public async Task<IActionResult> GetCommissionsById(int Id)
        {
            try
            {
                var commission = await _commissionRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<CommissionResponse>(commission)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("AddCommissions")]
        public async Task<IActionResult> AddCommissions([FromBody] CommissionRequest commissionRequest)
        {
            try
            {
                var commission = _mapper.Map<Commissions>(commissionRequest);
                commission.UserTypeID = (int)UserType.Buyer;
                commission.MarkAsDeleted = false;
                commission.CreatedAt = DateTime.Now;
                commission.CreatedBy = Convert.ToInt32(UserId);
                var result = await _commissionRepository.AddAsync(commission);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create Buyer Commissions"));
                }
                return Ok(new SharedResponse(true, 200, "Buyer Commission Created Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        //VAT Tax

        [HttpGet("GetAllVAT")]
        public async Task<IActionResult> GetAllVAT()
        {
            try
            {
                var vatList = (await _vatRepository.ListAllAsync()).
                    Where(x => x.MarkAsDeleted == false && x.UserTypeID == (int)UserType.Buyer).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<VATResponse>>(vatList)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetVATById")]
        public async Task<IActionResult> GetVATById(int Id)
        {
            try
            {
                var vat = await _vatRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<VATResponse>(vat)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("AddVAT")]
        public async Task<IActionResult> AddVAT([FromBody] VATRequest vatRequest)
        {
            try
            {
                var vat = _mapper.Map<VAT>(vatRequest);
                vat.UserTypeID = (int)UserType.Buyer;
                vat.MarkAsDeleted = false;
                vat.CreatedAt = DateTime.Now;
                vat.CreatedBy = Convert.ToInt32(UserId);
                var result = await _vatRepository.AddAsync(vat);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create Buyer VAT Tax"));
                }
                return Ok(new SharedResponse(true, 200, "Buyer VAT Created Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpGet("GetBuyerInvoice")]
        public async Task<IActionResult> GetProviderInvoice(long OrderID)
        {
            try
            {
                int UserTypeID = (int)UserType.Buyer;
                var BuyerInvoice = await _userRepository.GetBuyerInvoice(OrderID, UserTypeID);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<InvoiceResponse>(BuyerInvoice)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

    }
}
