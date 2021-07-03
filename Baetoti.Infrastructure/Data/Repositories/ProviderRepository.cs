using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class ProviderRepository : EFRepository<Provider>, IProviderRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public ProviderRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Provider> GetByUserID(long UserID)
        {
            return await _dbContext.Providers.Where(x => x.UserID == UserID).FirstOrDefaultAsync();
        }
    }
}
