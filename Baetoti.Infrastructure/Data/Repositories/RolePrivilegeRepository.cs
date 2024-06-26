﻿using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Response.Role;
using System.Linq;
using MoreLinq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class RolePrivilegeRepository : EFRepository<RolePrivilege>, IRolePrivilegeRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public RolePrivilegeRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MenuResponse>> GetAllMenuWithSubMenu()
        {
            return await (from m in _dbContext.Menus
                          select new MenuResponse
                          {
                              ID = m.ID,
                              Name = m.Name,
                              SelectedPrivileges = _dbContext.Privileges.Select(x => new PrivilegeResponse
                              {
                                  ID = x.ID,
                                  Name = x.Name
                              }).ToList(),
                              SelectedSubMenu = _dbContext.SubMenus.Where(x => x.MenuID == m.ID).Select(x => new SubMenuResponse
                              {
                                  ID = x.ID,
                                  Name = x.Name,
                                  SelectedPrivileges = _dbContext.Privileges.Select(x => new PrivilegeResponse
                                  {
                                      ID = x.ID,
                                      Name = x.Name
                                  }).ToList()
                              }).ToList()
                          }).ToListAsync();
        }

        public async Task<RolePrivilegeByIDResponse> GetRoleWithPrivileges(long id)
        {
            var roleAndPrivileges = await (from rp in _dbContext.RolePrivileges
                                           join r in _dbContext.Roles on rp.RoleID equals r.ID
                                           where r.ID == id
                                           select new
                                           {
                                               ID = r.ID,
                                               RoleName = r.Name
                                           }).FirstOrDefaultAsync();

            var menu = (from m in _dbContext.Menus
                        join rp in _dbContext.RolePrivileges on m.ID equals rp.MenuID
                        where rp.RoleID == id
                        select new MenuResponse
                        {
                            ID = m.ID,
                            Name = m.Name,
                            IsSelected = true
                        }).DistinctBy(x => x.ID).ToList();

            foreach (var item in menu)
            {
                var selectedPrivileges = await (from p in _dbContext.Privileges
                                                join rp in _dbContext.RolePrivileges on
                                                new { PrivilegeID = p.ID, MenuID = item.ID, SubMenuID = Convert.ToInt64(0), RoleID = id } equals
                                                new { PrivilegeID = rp.PrivilegeID, MenuID = rp.MenuID, SubMenuID = rp.SubMenuID, RoleID = rp.RoleID }
                                                select new PrivilegeResponse
                                                {
                                                    ID = p.ID,
                                                    Name = p.Name,
                                                    IsSelected = true
                                                }).ToListAsync();
                item.SelectedPrivileges = selectedPrivileges;

                var subMenu = (from sm in _dbContext.SubMenus
                               join rp in _dbContext.RolePrivileges on sm.ID equals rp.SubMenuID
                               where sm.MenuID == item.ID && sm.ID == rp.SubMenuID && rp.RoleID == id
                               select new SubMenuResponse
                               {
                                   ID = sm.ID,
                                   Name = sm.Name,
                                   IsSelected = true
                               }).DistinctBy(x => x.ID).ToList();

                foreach (var sm in subMenu)
                {
                    var selectedSubMenuPrivileges = await (from p in _dbContext.Privileges
                                                           join rp in _dbContext.RolePrivileges on
                                                           new { PrivilegeID = p.ID, MenuID = item.ID, SubMenuID = sm.ID } equals
                                                           new { PrivilegeID = rp.PrivilegeID, MenuID = rp.MenuID, SubMenuID = rp.SubMenuID }
                                                           where rp.RoleID == id
                                                           select new PrivilegeResponse
                                                           {
                                                               ID = p.ID,
                                                               Name = p.Name,
                                                               IsSelected = true
                                                           }).ToListAsync();
                    sm.SelectedPrivileges = selectedSubMenuPrivileges;
                }

                item.SelectedSubMenu = subMenu;
            }

            var roleResponse = new RolePrivilegeByIDResponse();
            roleResponse.ID = roleAndPrivileges.ID;
            roleResponse.RoleName = roleAndPrivileges.RoleName;
            roleResponse.Menu = menu;
            return roleResponse;
        }

        Task<List<RolePrivilegeResponse>> IRolePrivilegeRepository.GetAllRoleWithPrivileges()
        {
            return (from rp in _dbContext.RolePrivileges
                    join r in _dbContext.Roles on rp.RoleID equals r.ID
                    select new RolePrivilegeResponse
                    {
                        ID = rp.ID,
                        RoleName = r.Name,
                        CreatedDate = r.CreatedAt,
                        MenuAuthorization = "Access to all menu"
                    }).ToListAsync();
        }

    }
}
