using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Response.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class StoreRepository : EFRepository<Store>, IStoreRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public StoreRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
