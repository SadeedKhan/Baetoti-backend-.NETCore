using Baetoti.Core.Entites.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baetoti.Core.Entites
{
    [Table("User", Schema = "baetoti")]

    public partial class User : BaseEntity
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? JoiningDate { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public string Username { get; set; }
        public int GenderID { get; set; }
        public int ShiftID { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public int ReportTo { get; set; }
        public string Address { get; set; }
        public string RefreshToken { get; set; }
        public int UserStatus { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public int? LastPasswordChangedById { get; set; }
        public bool? IsPasswordUpdateRequired { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime RecordDateTime { get; set; }
    }
}
