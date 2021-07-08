namespace Baetoti.Shared.Request.User
{
    public class FilterRequest
    {
        public string UserType { get; set; }
        public string UserStatus { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public string Status { get; set; }
        public string Fence { get; set; }
        public string DateRange { get; set; }
    }
}
