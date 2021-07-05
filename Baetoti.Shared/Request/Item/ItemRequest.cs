using System.Collections.Generic;

namespace Baetoti.Shared.Request.Item
{
    public class ItemRequest
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string ArabicName { get; set; }

        public string Description { get; set; }

        public long CategoryID { get; set; }

        public long SubCategoryID { get; set; }

        public long UnitID { get; set; }

        public long ProviderID { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public List<ItemTagRequest> Tags { get; set; }

        public ItemRequest()
        {
            Tags = new List<ItemTagRequest>();
        }

    }

    public class ItemTagRequest
    {
        public long ID { get; set; }

        public string Name { get; set; }
    }

}
