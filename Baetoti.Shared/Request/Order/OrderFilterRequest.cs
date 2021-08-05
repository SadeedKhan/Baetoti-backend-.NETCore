namespace Baetoti.Shared.Request.Order
{
    public class OrderFilterRequest
    {
        public string UserType { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public long CategoryID { get; set; }
        public long SubCategoryID { get; set; }
        public string Fence { get; set; }
        public string DateRange { get; set; }
        public int OrderStatus { get; set; }
    }
}
