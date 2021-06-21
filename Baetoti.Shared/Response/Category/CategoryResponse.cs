using System;

namespace Baetoti.Shared.Response.Category
{
    public class CategoryResponse
    {
        public long ID { get; set; }

        public string CategoryName { get; set; }

        public string CategoryArabicName { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }

        public string Picture { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }
    }
}
