namespace Baetoti.Shared.Request.Driver
{
    public class DriverApprovalRequest
    {
        public long UserID { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }
}
