using Baetoti.Core.Entites.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Core.Entites
{
    public partial class Tags : BaseEntity
    {
        public string TagEnglish { get; set; }

        public string TagArabic { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }
    }
}
