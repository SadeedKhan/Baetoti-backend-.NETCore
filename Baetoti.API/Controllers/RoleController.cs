using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Request;
using Baetoti.Shared.Request.Delete;
using Baetoti.Shared.Request.Role;
using Baetoti.Shared.Response.Role;
using Baetoti.Shared.Response.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.API.Controllers
{
    public class RoleController : ApiBaseController
    {

        public readonly IRoleRepository _roleRepository;
        public readonly IRolePrivilegeRepository _rolePrivilegeRepository;
        public readonly IMapper _mapper;

        public RoleController(
            IRoleRepository roleRepository,
            IRolePrivilegeRepository rolePrivilegeRepository,
            IMapper mapper
            )
        {
            _roleRepository = roleRepository;
            _rolePrivilegeRepository = rolePrivilegeRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var roleList = (await _roleRepository.ListAllAsync()).ToList();
                return Ok(new SharedResponse(true, 200, "", _mapper.Map<List<RoleResponse>>(roleList)));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetAllRolePrivileges")]
        public async Task<IActionResult> GetAllRolePrivileges()
        {
            try
            {
                var roleList = await _rolePrivilegeRepository.GetAllRoleWithPrivileges();
                return Ok(new SharedResponse(true, 200, "", roleList));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long Id)
        {
            try
            {
                var role = await _rolePrivilegeRepository.GetRoleWithPrivileges(Id);
                return Ok(new SharedResponse(true, 200, "", role));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message, null));
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] RolePrivilegeRequest roleRequest)
        {
            try
            {
                var role = new Roles();
                role.Name = roleRequest.RoleName;
                role.CreatedAt = DateTime.Now;
                role.CreatedBy = Convert.ToInt32(UserId);
                var roleResult = await _roleRepository.AddAsync(role);
                if (roleResult == null)
                    return Ok(new SharedResponse(false, 400, "Unable To Create Role"));
                var rolePrivileges = new List<RolePrivilege>();
                foreach (var menu in roleRequest.Menu)
                {
                    if (menu.IsSelected)
                    {
                        foreach (var preVililege in menu.SelectedPrivileges.Where(x => x.IsSelected == true))
                        {
                            var rolePrivilege = new RolePrivilege();
                            rolePrivilege.CreatedAt = DateTime.Now;
                            rolePrivilege.CreatedBy = Convert.ToInt32(UserId);
                            rolePrivilege.RoleID = roleResult.ID;
                            rolePrivilege.MenuID = menu.ID;
                            rolePrivilege.PrivilegeID = preVililege.ID;
                            rolePrivileges.Add(rolePrivilege);
                        }
                        foreach (var subMenu in menu.SelectedSubMenu.Where(x => x.IsSelected == true))
                        {
                            foreach (var preVililege in subMenu.SelectedPrivileges.Where(x => x.IsSelected == true))
                            {
                                var rolePrivilege = new RolePrivilege();
                                rolePrivilege.CreatedAt = DateTime.Now;
                                rolePrivilege.CreatedBy = Convert.ToInt32(UserId);
                                rolePrivilege.RoleID = roleResult.ID;
                                rolePrivilege.MenuID = menu.ID;
                                rolePrivilege.SubMenuID = subMenu.ID;
                                rolePrivilege.PrivilegeID = preVililege.ID;
                                rolePrivileges.Add(rolePrivilege);
                            }
                        }
                    }
                }
                var rolePrivilegeResult = _rolePrivilegeRepository.AddRangeAsync(rolePrivileges);
                if (rolePrivilegeResult == null)
                {
                    return Ok(new SharedResponse(false, 400, "Role Created but Unable To Assign Privileges"));
                }
                return Ok(new SharedResponse(true, 200, "Role and Privileges Saved Succesfully"));
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] RolePrivilegeRequest roleRequest)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(roleRequest.ID);
                if (role != null)
                {
                    role.Name = roleRequest.RoleName;
                    role.LastUpdatedAt = DateTime.Now;
                    role.UpdatedBy = Convert.ToInt32(UserId);
                    await _roleRepository.UpdateAsync(role);
                    var existingRolePrivileges = (await _rolePrivilegeRepository.ListAllAsync()).Where(x => x.RoleID == role.ID);
                    await _rolePrivilegeRepository.DeleteRangeAsync(existingRolePrivileges.ToList());
                    var rolePrivileges = new List<RolePrivilege>();
                    foreach (var menu in roleRequest.Menu)
                    {
                        if (menu.IsSelected)
                        {
                            foreach (var preVililege in menu.SelectedPrivileges.Where(x => x.IsSelected == true))
                            {
                                var rolePrivilege = new RolePrivilege();
                                rolePrivilege.CreatedAt = DateTime.Now;
                                rolePrivilege.CreatedBy = Convert.ToInt32(UserId);
                                rolePrivilege.RoleID = role.ID;
                                rolePrivilege.MenuID = menu.ID;
                                rolePrivilege.PrivilegeID = preVililege.ID;
                                rolePrivileges.Add(rolePrivilege);
                            }
                            foreach (var subMenu in menu.SelectedSubMenu.Where(x => x.IsSelected == true))
                            {
                                foreach (var preVililege in subMenu.SelectedPrivileges.Where(x => x.IsSelected == true))
                                {
                                    var rolePrivilege = new RolePrivilege();
                                    rolePrivilege.CreatedAt = DateTime.Now;
                                    rolePrivilege.CreatedBy = Convert.ToInt32(UserId);
                                    rolePrivilege.RoleID = role.ID;
                                    rolePrivilege.MenuID = menu.ID;
                                    rolePrivilege.SubMenuID = subMenu.ID;
                                    rolePrivilege.PrivilegeID = preVililege.ID;
                                    rolePrivileges.Add(rolePrivilege);
                                }
                            }
                        }
                    }
                    var rolePrivilegeResult = _rolePrivilegeRepository.AddRangeAsync(rolePrivileges);
                    if (rolePrivilegeResult == null)
                    {
                        return Ok(new SharedResponse(false, 400, "Role Updated but Unable To Assign Privileges"));
                    }
                    return Ok(new SharedResponse(true, 200, "Role and Privileges Updated Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "unable to find role"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteRequest deleteRequest)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(deleteRequest.ID);
                if (role != null)
                {
                    await _roleRepository.DeleteAsync(role);
                    var existingRolePrivileges = (await _rolePrivilegeRepository.ListAllAsync()).Where(x => x.RoleID == role.ID);
                    await _rolePrivilegeRepository.DeleteRangeAsync(existingRolePrivileges.ToList());
                    return Ok(new SharedResponse(true, 200, "Role & Privileges Deleted Succesfully"));
                }
                else
                {
                    return Ok(new SharedResponse(false, 400, "Unable To Find Role & Privileges"));
                }
            }
            catch (Exception ex)
            {
                return Ok(new SharedResponse(false, 400, ex.Message));
            }
        }

    }
}
