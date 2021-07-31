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

        public async Task<List<StoreResponse>> GetAllByUserId(long id)
        {
            //var p = from s in _dbContext.Stores
            //        join u in _dbContext.Users on p.UserID equals s.ID
            //        select new StoreResponse
            //        {
            //           ID=s.ID,
            //           Name=s.Name,
                       
            //        };
            return null;
        }
    }
}
