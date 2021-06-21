﻿using Baetoti.Core.Entites.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Baetoti.Core.Entites
{
    [Table("Units", Schema = "baetoti")]
    public partial class Units : BaseEntity
    {

        public string Family { get; set; }

        public string UnitName { get; set; }

        public string UnitArabicName { get; set; }

        public string UnitEnglishName { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }
    }
}
