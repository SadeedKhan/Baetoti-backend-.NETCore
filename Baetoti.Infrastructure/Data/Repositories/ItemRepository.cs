using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Response.Item;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Baetoti.Shared.Enum;

namespace Baetoti.Infrastructure.Data.Repositories
{
    public class ItemRepository : EFRepository<Item>, IItemRepository
    {

        private readonly BaetotiDbContext _dbContext;

        public ItemRepository(BaetotiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ItemResponse> GetAll()
        {
            var itemList = await (from i in _dbContext.Items
                                  join c in _dbContext.Categories on i.CategoryID equals c.ID
                                  join sc in _dbContext.SubCategories on i.SubCategoryID equals sc.ID
                                  select new ItemListResponse
                                  {
                                      ID = i.ID,
                                      ProviderID = i.ProviderID,
                                      Title = i.Name,
                                      Category = c.CategoryName,
                                      SubCategory = sc.SubCategoryName,
                                      OrderedQuantity = 0,
                                      Revenue = 0,
                                      AveragePreparationTime = "0",
                                      CreateDate = i.CreatedAt,
                                      Status = ((ItemStatus)i.Status).ToString(),
                                      Price = $"{i.Price} SAR/{ _dbContext.Units.Where(x => x.ID == i.UnitID).FirstOrDefault().UnitEnglishName}",
                                      Rating = 0
                                  }).ToListAsync();
            var itemResponse = new ItemResponse();
            itemResponse.items = itemList;
            itemResponse.Total = itemList.Count();
            itemResponse.Active = itemList.Count(x => x.Status == "Active");
            itemResponse.Inactive = itemList.Count(x => x.Status == "Inactive");
            itemResponse.Flaged = 0; //itemList.Count();
            return itemResponse;
        }

        public async Task<ItemResponseByID> GetByID(long ItemID)
        {
            var item = await (from i in _dbContext.Items
                              join c in _dbContext.Categories on i.CategoryID equals c.ID
                              join sc in _dbContext.SubCategories on i.SubCategoryID equals sc.ID
                              join p in _dbContext.Providers on i.ProviderID equals p.ID
                              //join u in _dbContext.Users on p.UserID equals u.ID
                              where i.ID == ItemID
                              select new ItemResponseByID
                              {
                                  ID = i.ID,
                                  StoreName = "",
                                  Location = "",
                                  Title = i.Name,
                                  Description = i.Description,
                                  Category = c.CategoryName,
                                  SubCategory = sc.SubCategoryName,
                                  Quantity = 0,
                                  TotalRevenue = 0,
                                  AveragePreparationTime = "0",
                                  Price = $"{i.Price} SAR/{ _dbContext.Units.Where(x => x.ID == i.UnitID).FirstOrDefault().UnitEnglishName}",
                                  AverageRating = 0,
                                  Unit = "",
                                  Sold = 0,
                                  AvailableNow = 0,
                                  Tags = _dbContext.Tags.Join(_dbContext.ItemTags.Where(x => x.ItemID == i.ID),
                                                  t => t.ID,
                                                  it => it.ItemID,
                                                  (t, it) => new TagResponse
                                                  {
                                                      ID = t.ID,
                                                      Name = t.TagEnglish
                                                  }).ToList()
                                  //Reviews = new List<ReviewResponse>()
                              }).FirstOrDefaultAsync();

            return item;
        }
    }
}
