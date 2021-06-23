using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Core.Interface.Services;
using Baetoti.Shared.Request.User;
using Baetoti.Shared.Request.UserRole;
using Baetoti.Shared.Response.Shared;
using Baetoti.Shared.Response.User;
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
        public readonly IUserRoleRepository _userroleRepository;
        private readonly IArgon2Service _hashingService;
        public readonly IMapper _mapper;

        public UserController(
         IUserRepository userRepository,
         IUserRoleRepository userroleRepository,
         IArgon2Service hashingService,
         IMapper mapper
         )
        {
            _userRepository = userRepository;
            _userroleRepository = userroleRepository;
            _hashingService = hashingService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userList = (await _userRepository.ListAllAsync()).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<UserResponse>>(userList)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(Id);
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<UserResponse>(user)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] UserRequest userRequest)
        {
            try
            {

                var user = _mapper.Map<User>(userRequest);
                user.Password = _hashingService.GenerateHash(userRequest.Password);
                user.CreatedAt = DateTime.Now;
                user.CreatedBy = Convert.ToInt32(UserId);
                user.UserStatus = 1; //Change it after Email or mobile verifications
                var result = await _userRepository.AddAsync(user);
                if (result == null)
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Create User"));
                }
                else
                {
                    AssignRoleRequest assignRole = new AssignRoleRequest();
                    assignRole.RoleId = userRequest.RoleID;
                    assignRole.UserId = Convert.ToInt32(result.ID);
                    var role = _mapper.Map<UserRoles>(assignRole);
                    role.CreatedAt = DateTime.Now;
                    role.CreatedBy = Convert.ToInt32(UserId);
                    var res = await _userroleRepository.AddAsync(role);
                }
                return Ok(new SharedResponse(true, 200, "User Created Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] UserRequest userRequest)
        {
            try
            {
                var cat = await _userRepository.GetByIdAsync(userRequest.ID);
                if (cat != null)
                {
                    var user = _mapper.Map<User>(userRequest);
                    user.LastUpdatedAt = DateTime.Now;
                    user.LastUpdatedBy = Convert.ToInt32(UserId);
                     await _userRepository.UpdateAsync(user);
                    return Ok(new SharedResponse(true, 200, "User Updated Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find User"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] long ID)
        {
            try
            {
                var un = await _userRepository.GetByIdAsync(ID);
                if (un != null)
                {
                    await _userRepository.DeleteAsync(un);
                    return Ok(new SharedResponse(true, 200, "User Deleted Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find User"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }
    }
}
