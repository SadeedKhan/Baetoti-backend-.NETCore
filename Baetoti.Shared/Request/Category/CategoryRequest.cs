using System;

namespace Baetoti.Shared.Request.Category
{
    public class CategoryRequest
    {
        public long ID { get; set; }

        public string CategoryName { get; set; }

        public string CategoryArabicName { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }

        public string Picture { get; set; }
    }
}
