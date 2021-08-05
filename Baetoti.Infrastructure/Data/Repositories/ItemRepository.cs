using Baetoti.Core.Entites;
using Baetoti.Core.Interface.Repositories;
using Baetoti.Infrastructure.Data.Context;
using Baetoti.Infrastructure.Data.Repositories.Base;
using Baetoti.Shared.Response.Item;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Baetoti.Shared.Enum;
using Baetoti.Shared.Request.Item;
using System;

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
                                  where i.MarkAsDeleted == false
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

        public async Task<ItemResponse> GetFilteredItemsDataAsync(ItemFilterRequest filterRequest)
        {
            var itemList = await (from i in _dbContext.Items
                                  join c in _dbContext.Categories on i.CategoryID equals c.ID
                                  join sc in _dbContext.SubCategories on i.SubCategoryID equals sc.ID
                                  where i.MarkAsDeleted == false && i.Name == filterRequest.Name &&
                                  i.ArabicName == filterRequest.ArabicName && i.Price == filterRequest.Price
                                  && i.CategoryID == filterRequest.CategoryID && i.SubCategoryID == filterRequest.SubCategoryID
                                  && i.UnitID == filterRequest.UnitID
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
                                      Rating = 0,
                                  }).ToListAsync();
            var itemResponse = new ItemResponse();
            itemResponse.items = itemList;
            //itemResponse.Total = itemList.Count();
            //itemResponse.Active = itemList.Count(x => x.Status == "Active");
            //itemResponse.Inactive = itemList.Count(x => x.Status == "Inactive");
            //itemResponse.Flaged = 0; //itemList.Count();
            return itemResponse;
        }

        public async Task<ItemResponseByID> GetByID(long ItemID)
        {
            var item = await (from i in _dbContext.Items
                              join c in _dbContext.Categories on i.CategoryID equals c.ID
                              join sc in _dbContext.SubCategories on i.SubCategoryID equals sc.ID
                              join p in _dbContext.Providers on i.ProviderID equals p.ID
                              join u in _dbContext.Users on p.UserID equals u.ID
                              join un in _dbContext.Units on i.UnitID equals un.ID
                              join s in _dbContext.Stores on p.ID equals s.ProviderID
                              where i.ID == ItemID
                              select new ItemResponseByID
                              {
                                  ID = i.ID,
                                  StoreName = s.Name,
                                  Location = s.Location,
                                  Title = i.Name,
                                  Description = i.Description,
                                  CategoryID = i.CategoryID,
                                  Category = c.CategoryName,
                                  SubCategoryID = i.SubCategoryID,
                                  SubCategory = sc.SubCategoryName,
                                  Quantity = 0,
                                  Picture = i.Picture,
                                  TotalRevenue = 0,
                                  AveragePreparationTime = "0",
                                  Price = $"{i.Price} SAR/{ _dbContext.Units.Where(x => x.ID == i.UnitID).FirstOrDefault().UnitEnglishName}",
                                  AverageRating = 0,
                                  UnitID = i.UnitID,
                                  Unit = un.UnitEnglishName,
                                  Sold = 0,
                                  AvailableNow = 0,
                                  Tags = (from t in _dbContext.Tags
                                          join it in _dbContext.ItemTags
                                          on t.ID equals it.ItemID
                                          where it.ItemID == ItemID
                                          select new TagResponse
                                          {
                                              ID = t.ID,
                                              Name = t.TagEnglish
                                          }).ToList(),
                                  Reviews = (from ir in _dbContext.ItemReviews
                                             join u in _dbContext.Users on ir.UserID equals u.ID
                                             where ir.ItemID == ItemID
                                             select new ReviewResponse
                                             {
                                                 UserName = $"{u.FirstName} {u.LastName}",
                                                 Picture = u.Picture,
                                                 Rating = ir.Rating,
                                                 Reviews = ir.Review,
                                                 ReviewDate = ir.RecordDateTime
                                             }).ToList(),
                                  RecentOrder = (from O in _dbContext.Orders
                                                 join oi in _dbContext.OrderItems on O.ID equals oi.OrderID
                                                 join i in _dbContext.Items on oi.ItemID equals i.ID
                                                 join u in _dbContext.Users on O.UserID equals u.ID
                                                 where i.ID == ItemID
                                                 select new RecentOrder
                                                 {
                                                     OrderID = Convert.ToInt32(O.ID),
                                                     Driver = $"{u.FirstName} {u.LastName}",
                                                     Buyer = O.Rating.ToString(),
                                                     PickUp = O.OrderPickUpTime,
                                                     Delivery = O.ActualDeliveryTime,
                                                     Rating = O.Rating
                                                 }).OrderByDescending(x => x.OrderID).Take(10).ToList(),
                              }).FirstOrDefaultAsync();

            return item;
        }
    }
}
