using Baetoti.Core.Entites.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baetoti.Core.Entites
{
    [Table("User", Schema = "baetoti")]

    public partial class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public int UserStatus { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public int? LastPasswordChangedById { get; set; }
        public bool? IsPasswordUpdateRequired { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime RecordDateTime { get; set; }
    }
}
