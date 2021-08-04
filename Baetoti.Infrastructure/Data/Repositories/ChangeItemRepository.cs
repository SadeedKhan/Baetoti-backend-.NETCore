using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Response.ChangeItem;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class ChangeItemRepository : EFRepository<ChangeItem>, IChangeItemRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public ChangeItemRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChangeItemResponseByID> GetByItemID(long ItemID)
        {
            var changeitem = await (from ci in _dbContext.ChangeItem
                                    join c in _dbContext.Categories on ci.CategoryID equals c.ID
                                    join sc in _dbContext.SubCategories on ci.SubCategoryID equals sc.ID
                                    join p in _dbContext.Providers on ci.ProviderID equals p.ID
                                    join u in _dbContext.Units on ci.UnitID equals u.ID
                                    //join u in _dbContext.Users on p.UserID equals u.ID
                                    where ci.ItemId == ItemID
                                    select new ChangeItemResponseByID
                                    {
                                        ID = ci.ID,
                                        ItemId = ci.ItemId,
                                        StoreName = "",
                                        Location = "",
                                        Title = ci.Name,
                                        Description = ci.Description,
                                        CategoryID=ci.CategoryID,
                                        Category = c.CategoryName,
                                        SubCategoryID = ci.SubCategoryID,
                                        SubCategory = sc.SubCategoryName,
                                        Quantity = 0,
                                        TotalRevenue = 0,
                                        AveragePreparationTime = "0",
                                        Price = $"{ci.Price} SAR/{ _dbContext.Units.Where(x => x.ID == ci.UnitID).FirstOrDefault().UnitEnglishName}",
                                        AverageRating = 0,
                                        UnitID = ci.UnitID,
                                        Unit =u.UnitEnglishName,
                                        Sold = 0,
                                        AvailableNow = 0,
                                        Tags = (from t in _dbContext.Tags
                                                join cit in _dbContext.ChangeItemTag
                                                on t.ID equals cit.ItemID
                                                where cit.ItemID == ItemID
                                                select new ChanngeItemTagResponse
                                                {
                                                    ID = t.ID,
                                                    Name = t.TagEnglish
                                                }).ToList(),
                                        Reviews = (from ir in _dbContext.ItemReviews
                                                   join u in _dbContext.Users on ir.UserID equals u.ID
                                                   where ir.ItemID == ItemID
                                                   select new ChangeItemReviewResponse
                                                   {
                                                       UserName = $"{u.FirstName} {u.LastName}",
                                                       Picture = u.Picture,
                                                       Rating = ir.Rating,
                                                       Reviews = ir.Review,
                                                       ReviewDate = ir.RecordDateTime
                                                   }).ToList()
                                    }).FirstOrDefaultAsync();

            return changeitem;
            //}
        }
    }
}
