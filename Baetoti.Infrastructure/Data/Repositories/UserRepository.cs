using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
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

        public async Task<User> GetByMobileNumberAsync(string mobileNumber)
        {
            return await _dbContext.Users.Where(x => x.Phone == mobileNumber).FirstOrDefaultAsync();
        }
    }
}
