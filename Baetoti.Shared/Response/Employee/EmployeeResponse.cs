using System;

namespace Baetoti.Shared.Response.Employee
{
    public class EmployeeResponse
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string Location { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string Shift { get; set; }
        public string Email { get; set; }
        public DateTime? DOB { get; set; }
        public string Phone { get; set; }
        public int ReportTo { get; set; }
        public string Address { get; set; }
        public string Goals { get; set; }
        public string Skills { get; set; }
        public string RefreshToken { get; set; }
        public int UserStatus { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
