using System;

namespace Baetoti.Shared.Request.Provider
{
    public class ProviderRequest
    {
        public long UserID { get; set; }
        public string MaroofID { get; set; }
        public string GovernmentID { get; set; }
        public string GovernmentIDPicture { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
