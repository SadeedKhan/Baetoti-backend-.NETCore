using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Response.SubCategory
{
    public class SubCategoryResponse
    {
        public long ID { get; set; }

        public long CategoryID { get; set; }

        public string SubCategoryName { get; set; }

        public string SubCategoryArabicName { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }

        public string Picture { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }
    }
}
