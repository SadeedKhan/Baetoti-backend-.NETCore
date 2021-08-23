﻿using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class VATRepository : EFRepository<VAT>, IVATRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public VATRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
