using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class DriverRepository : EFRepository<Driver>, IDriverRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public DriverRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Driver> GetByUserID(long UserID)
        {
            return await _dbContext.Drivers.Where(x => x.UserID == UserID).FirstOrDefaultAsync();
        }
    }
}
