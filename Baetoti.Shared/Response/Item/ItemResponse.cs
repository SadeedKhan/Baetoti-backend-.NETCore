using System;
using System.Collections.Generic;

namespace Baetoti.Shared.Response.Item
{
    public class ItemResponse
    {
        public int Total { get; set; }
        public int Active { get; set; }
        public int Inactive { get; set; }
        public int Flaged { get; set; }
        public List<ItemListResponse> items { get; set; }
    }

    public class ItemListResponse
    {
        public long ID { get; set; }
        public long ProviderID { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public long OrderedQuantity { get; set; }
        public long Revenue { get; set; }
        public string AveragePreparationTime { get; set; }
        public string Status { get; set; }
        public string Price { get; set; }
        public decimal Rating { get; set; }
        public DateTime? CreateDate { get; set; }
    }

}
