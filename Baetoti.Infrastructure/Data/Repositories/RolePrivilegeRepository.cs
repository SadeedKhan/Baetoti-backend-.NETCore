using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class RolePrivilegeRepository : EFRepository<RolePrivilege>, IRolePrivilegeRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public RolePrivilegeRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
