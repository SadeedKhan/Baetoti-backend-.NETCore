using Baetoti.Core.Entites.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baetoti.Core.Entites
{
    [Table("Provider", Schema = "baetoti")]
    public class Provider : BaseEntity
    {
        public long UserID { get; set; }
        public string MaroofID { get; set; }
        public string GovernmentID { get; set; }
        public string GovernmentIDPicture { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int ProviderStatus { get; set; }
        public string Comments { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool MarkAsDeleted { get; set; }
    }
}
