﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Request.User
{
    public class UserRequest
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
        public string Goals { get; set; }
        public string Skills { get; set; }
    }
}
