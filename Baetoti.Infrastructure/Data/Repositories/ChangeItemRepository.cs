using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class ChangeItemRepository : EFRepository<ChangeItem>, IChangeItemRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public ChangeItemRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
