using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class UserRoleRepository : EFRepository<UserRoles>, IUserRoleRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public UserRoleRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        async Task<UserRoles> IUserRoleRepository.GetByUserId(int UserId)
        {
            return await _dbContext.UserRoles.Where(x => x.UserId == UserId).FirstOrDefaultAsync();
        }

    }
}
