﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Response.ChangeItem
{
   public class ChangeItemResponseByID
    {
        public long ID { get; set; }
        public long ItemId { get; set; }
        public string StoreName { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public long CategoryID { get; set; }
        public string Category { get; set; }
        public long SubCategoryID { get; set; }
        public string SubCategory { get; set; }
        public string Price { get; set; }
        public long UnitID { get; set; }
        public string Unit { get; set; }
        public long Sold { get; set; }
        public long AvailableNow { get; set; }
        public long Quantity { get; set; }
        public long TotalRevenue { get; set; }
        public string AveragePreparationTime { get; set; }
        public string Description { get; set; }
        public List<ChanngeItemTagResponse> Tags { get; set; }
        public List<ChangeItemReviewResponse> Reviews { get; set; }
        public decimal AverageRating { get; set; }
    }

    public class ChangeItemReviewResponse
    {
        public string UserName { get; set; }
        public string Picture { get; set; }
        public decimal Rating { get; set; }
        public string Reviews { get; set; }
        public DateTime ReviewDate { get; set; }
    }

    public class ChanngeItemTagResponse
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }
}