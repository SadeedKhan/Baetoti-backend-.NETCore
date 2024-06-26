﻿using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Response.SubCategory;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class SubCategoryRepository : EFRepository<SubCategory>, ISubCategoryRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public SubCategoryRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        Task<List<SubCategoryResponse>> ISubCategoryRepository.GetByCategoryAsync(long id)
        {
            return (from c in _dbContext.Categories
                    join sc in _dbContext.SubCategories
                    on c.ID equals sc.CategoryId
                    where (id == 0 || sc.CategoryId == id)
                    && c.MarkAsDeleted == false
                    && sc.MarkAsDeleted == false
                    select new SubCategoryResponse
                    {
                        ID = sc.ID,
                        CategoryID = sc.CategoryId,
                        CategoryName = c.CategoryName,
                        CategoryArabicName = c.CategoryArabicName,
                        SubCategoryName = sc.SubCategoryName,
                        SubCategoryArabicName = sc.SubCategoryArabicName
                    }).ToListAsync();
        }
    }
}
