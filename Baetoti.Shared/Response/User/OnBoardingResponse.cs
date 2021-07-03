using System;

namespace Baetoti.Shared.Response.User
{
    public class OnBoardingResponse
    {
        public long UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
    }

    public class ProviderApproval
    {
        public long UserID { get; set; }
        public string MaroofID { get; set; }
        public string GovernmentID { get; set; }
        public string GovernmentIDPicture { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class ProviderApprovalStatus
    {
        public long UserID { get; set; }
        public bool IsApproved { get; set; }
    }

}
