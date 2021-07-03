using System.Collections.Generic;

namespace Baetoti.Shared.Response.Item
{
    public class ItemResponse
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string ArabicName { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public string Reviews { get; set; }

        public long CategoryID { get; set; }

        public long SubCategoryID { get; set; }

        public long UnitID { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public List<ItemTagResponse> Tags { get; set; }

        public ItemResponse()
        {
            Tags = new List<ItemTagResponse>();
        }
    }

    public class ItemTagResponse
    {
        public long ID { get; set; }

        public string Name { get; set; }
    }

}
