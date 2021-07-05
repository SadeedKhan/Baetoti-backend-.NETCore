using System;
using System.Collections.Generic;

namespace Baetoti.Shared.Response.Item
{
    public class ItemResponseByID
    {
        public long ID { get; set; }
        public string StoreName { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Price { get; set; }
        public string Unit { get; set; }
        public long Sold { get; set; }
        public long AvailableNow { get; set; }
        public long Quantity { get; set; }
        public long TotalRevenue { get; set; }
        public string AveragePreparationTime { get; set; }
        public string Description { get; set; }
        public List<TagResponse> Tags { get; set; }
        public List<ReviewResponse> Reviews { get; set; }
        public decimal AverageRating { get; set; }
    }

    public class RecentOrder
    {
        public int OrderID { get; set; }
        public string Driver { get; set; }
        public string Buyer { get; set; }
        public DateTime PickUp { get; set; }
        public DateTime Delivery { get; set; }
        public decimal Rating { get; set; }
    }

    public class ReviewResponse
    {
        public string UserName { get; set; }
        public string Picture { get; set; }
        public decimal Rating { get; set; }
        public string Reviews { get; set; }
        public DateTime ReviewDate { get; set; }
    }

    public class TagResponse
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }
}
