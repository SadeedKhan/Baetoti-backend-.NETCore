using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Core.Interface.Services;
using Baetoti.Shared.Request.Auth;
using Baetoti.Shared.Response.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class AuthController : ApiBaseController
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IRijndaelEncryptionService _hashingService;

        public AuthController(IEmployeeRepository employeeRepository,
                IJwtService jwtService,
                IMapper mapper,
                IRijndaelEncryptionService hashingService)
        {
            _employeeRepository = employeeRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _hashingService = hashingService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            try
            {
                var employee = await _employeeRepository.AuthenticateUser(_mapper.Map<Employee>(request));
                if (employee != null)
                {
                    var decryptedPassword = _hashingService.DecryptPassword(employee.Password, employee.Salt);
                    if (decryptedPassword == request.Password)
                    {
                        employee.LastLogin = DateTime.UtcNow;
                        await _employeeRepository.UpdateAsync(employee);
                        var response = await _jwtService.GenerateTokenAsync(employee);
                        return Ok(new SharedResponse(true, 200, "Success", response));
                    }
                    return Ok(new SharedResponse(false, 400, "Invalid Password."));
                }
                return Ok(new SharedResponse(false, 400, "Invalid Username."));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> logout([FromBody] long UserID)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(UserID);
                employee.RefreshToken = string.Empty;
                await _employeeRepository.UpdateAsync(employee);
                return Ok(new SharedResponse(false, 400, "Logout Successfully."));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(request?.Token);
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }
            var result = int.TryParse(principal.Identity.Name, out int userId);
            if (!result)
            {
                throw new ArgumentException(nameof(userId));
            }
            var user = await _employeeRepository.GetByIdAsync(userId);
            if (user.RefreshToken != request.RefreshToken)
                return BadRequest(new SharedResponse(false, 400, "Invalid Refresh token", null));
            var response = await _jwtService.GenerateTokenAsync(user);
            return Ok(new SharedResponse(true, 200, "Success", response));
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = await _employeeRepository.GetByIdAsync(Convert.ToInt32(UserId));
            if (user != null)
            {
                var decruptedPassword = _hashingService.DecryptPassword(user.Password, user.Salt);
                if (decruptedPassword == request.CurrentPassword)
                {
                    var salt = _hashingService.GenerateSalt(8, 10);
                    var newHash = _hashingService.EncryptPassword(request.NewPassword, salt);
                    user.Password = newHash;
                    user.Salt = salt;
                    user.LastPasswordChangedById = Convert.ToInt32(UserId);
                    user.LastPasswordChangedDate = DateTime.Now;
                    await _employeeRepository.UpdateAsync(user);
                    return Ok(new SharedResponse(true, 200, "Success"));
                }
                return Ok(new SharedResponse(false, 400, "Invalid Current Password."));
            }
            return Ok(new SharedResponse(false, 400, "Please try later."));
        }

    }
}
