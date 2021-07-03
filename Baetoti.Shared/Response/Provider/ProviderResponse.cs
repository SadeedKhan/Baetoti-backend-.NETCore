using System;

namespace Baetoti.Shared.Response.Provider
{
    public class ProviderResponse
    {
        public long UserID { get; set; }
        public string MaroofID { get; set; }
        public string GovernmentID { get; set; }
        public string GovernmentIDPicture { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
