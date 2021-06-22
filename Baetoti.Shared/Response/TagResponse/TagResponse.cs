using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Response.TagResponse
{
    public class TagResponse
    {
        public long ID { get; set; }

        public string TagEnglish { get; set; }

        public string TagArabic { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }
    }
}
