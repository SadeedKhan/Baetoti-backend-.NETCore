using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Core.Interface.Services;
using Baetoti.Shared.Request.Auth;
using Baetoti.Shared.Response.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class AuthController : ApiBaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IArgon2Service _hashingService;

        public AuthController(IUserRepository userRepository,
                IJwtService jwtService,
                IMapper mapper,
                IArgon2Service hashingService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _hashingService = hashingService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            try
            {
                var user = await _userRepository.AuthenticateUser(_mapper.Map<User>(request));
                if (user != null)
                {
                    var IsAuthenticated = _hashingService.VerifyHash(request.Password, user.Password);
                    if (IsAuthenticated)
                    {
                        user.LastLogin = DateTime.UtcNow;
                        await _userRepository.UpdateAsync(user);
                        var response = await _jwtService.GenerateTokenAsync(user);
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
            var user = await _userRepository.GetByIdAsync(userId);
            if (user.RefreshToken != request.RefreshToken)
                return BadRequest(new SharedResponse(false, 400, "Invalid Refresh token", null));
            var response = await _jwtService.GenerateTokenAsync(user);
            return Ok(new SharedResponse(true, 200, "Success", response));
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var user = await _userRepository.GetByIdAsync(Convert.ToInt32(UserId));
            if (user != null)
            {
                var IsAuthenticated = _hashingService.VerifyHash(request.CurrentPassword, user.Password);
                if (IsAuthenticated)
                {
                    var newHash = _hashingService.GenerateHash(request.NewPassword);
                    user.Password = newHash;
                    user.LastPasswordChangedById = Convert.ToInt32(UserId);
                    user.LastPasswordChangedDate = DateTime.Now;
                    await _userRepository.UpdateAsync(user);
                    return Ok(new SharedResponse(true, 200, "Success"));
                }
                return Ok(new SharedResponse(false, 400, "Invalid Current Password."));
            }
            return Ok(new SharedResponse(false, 400, "Please try later."));
        }

    }
}
