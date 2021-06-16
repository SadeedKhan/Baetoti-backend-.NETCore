using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public UserRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        async Task<List<string>> IUserRepository.GetRolesAsync(User user)
        {
            return await (from ur in _dbContext.UserRoles
                          join
                          r in _dbContext.Roles on ur.RoleId equals r.ID
                          where ur.UserId == user.ID
                          select r.Name).ToListAsync();
        }

        async Task<User> IUserRepository.AuthenticateUser(User user)
        {
            return await _dbContext.Users.Where(x => x.Username.ToLower() == user.Username.ToLower() && x.UserStatus == (int)EmployementStatus.Active).FirstOrDefaultAsync();
        }

    }
}
