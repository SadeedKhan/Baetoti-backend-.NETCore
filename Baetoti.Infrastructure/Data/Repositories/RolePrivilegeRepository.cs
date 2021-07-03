using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Response.Role;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class RolePrivilegeRepository : EFRepository<RolePrivilege>, IRolePrivilegeRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public RolePrivilegeRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<RolePrivilegeResponse> GetRoleWithPrivileges(long id)
        {
            throw new System.NotImplementedException();
        }

        Task<List<RolePrivilegeResponse>> IRolePrivilegeRepository.GetAllRoleWithPrivileges()
        {
            return (from rp in _dbContext.RolePrivileges
                    join r in _dbContext.Roles on rp.RoleID equals r.ID
                    join m in _dbContext.Menus on rp.MenuID equals m.ID
                    join sm in _dbContext.SubMenus on rp.SubMenuID equals sm.ID
                    join p in _dbContext.Privileges on rp.PrivilegeID equals p.ID
                    //into temp
                    select new RolePrivilegeResponse
                    {
                        ID = rp.ID,
                        RoleName = r.Name,
                        CreatedDate = r.CreatedAt,
                        //Menu = temp.Select(x => x).ToList()
                    }).ToListAsync();
            //return (from rp in _dbContext.RolePrivileges
            //        from r in _dbContext.Roles
            //        where r.ID == rp.RoleID
            //        from m in _dbContext.Menus
            //        where m.ID == rp.MenuID
            //        from sm in _dbContext.SubMenus
            //        where sm.MenuID == m.ID
            //        from p in _dbContext.Privileges
            //        where p.ID == rp.ID
            //        select new RolePrivilegeResponse
            //        {
            //            ID = rp.ID,
            //            RoleName = r.Name,
            //            CreatedDate = r.CreatedAt,
            //            Menu = m.
            //        }).ToListAsync();
        }

    }
}
