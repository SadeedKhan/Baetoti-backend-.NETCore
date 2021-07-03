using System;
using System.Collections.Generic;

namespace Baetoti.Shared.Response.User
{
    public class OnBoardingResponse
    {
        public ProviderOnBoardingRequest providers { get; set; }
        public DriverOnBoardingRequest drivers { get; set; }
        public ProviderAndDriverOnBoardingRequest providerAndDrivers { get; set; }
    }

    public class OnBoardingUserList
    {
        public long UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public DateTime? RequestDate { get; set; }
    }

    public class UserStates
    {
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
    }

    public class DriverOnBoardingRequest
    {
        public UserStates userStates { get; set; }
        public List<OnBoardingUserList> userList { get; set; }
    }

    public class ProviderOnBoardingRequest
    {
        public UserStates userStates { get; set; }
        public List<OnBoardingUserList> userList { get; set; }
    }
    public class ProviderAndDriverOnBoardingRequest
    {
        public UserStates userStates { get; set; }
        public List<OnBoardingUserList> userList { get; set; }
    }

}
