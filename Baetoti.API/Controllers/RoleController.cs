using AutoMapper;
using Baetoti.API.Controllers.Base;
using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Shared.Request;
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

        //[HttpGet("GetById")]
        //public async Task<IActionResult> GetById(int Id)
        //{
        //    try
        //    {
        //        var category = await _categoryRepository.GetByIdAsync(Id);
        //        return Ok(new SharedResponse(true, 200, "", _mapper.Map<CategoryResponse>(category)));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new SharedResponse(false, 400, ex.Message, null));
        //    }
        //}

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] RoleRequest roleRequest)
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

        //[HttpPost("Update")]
        //public async Task<IActionResult> Update([FromBody] CategoryRequest categoryRequest)
        //{
        //    try
        //    {
        //        var cat = await _categoryRepository.GetByIdAsync(categoryRequest.ID);
        //        if (cat != null)
        //        {

        //            var category = _mapper.Map<Category>(categoryRequest);
        //            category.LastUpdatedAt = DateTime.Now;
        //            category.UpdatedBy = Convert.ToInt32(UserId);
        //            await _categoryRepository.UpdateAsync(category);
        //            return Ok(new SharedResponse(true, 200, "Category Updated Succesfully"));
        //        }
        //        else
        //        {
        //            return Ok(new SharedResponse(false, 400, "unable to find category"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new SharedResponse(false, 400, ex.Message));
        //    }
        //}

        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete([FromBody] DeleteRequest deleteRequest)
        //{
        //    try
        //    {
        //        var cat = await _categoryRepository.GetByIdAsync(deleteRequest.ID);
        //        if (cat != null)
        //        {
        //            await _categoryRepository.DeleteAsync(cat);
        //            return Ok(new SharedResponse(true, 200, "Category Deleted Succesfully"));
        //        }
        //        else
        //        {
        //            return Ok(new SharedResponse(false, 400, "Unable To Find Category"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new SharedResponse(false, 400, ex.Message));
        //    }
        //}

    }
}
