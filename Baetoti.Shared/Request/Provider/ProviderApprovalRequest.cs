namespace Baetoti.Shared.Request.Provider
{
    public class ProviderApprovalRequest
    {
        public long UserID { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }
}
