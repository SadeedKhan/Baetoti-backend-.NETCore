namespace Baetoti.Shared.Response.Order
{
    public class OrderByIDResponse
    {
        public OrderDetail orderDetail { get; set; }
        public CustomerDetail customerDetail { get; set; }
        public DriverDetail driverDetail { get; set; }
        public ProviderDetail providerDetail { get; set; }
        public PaymentInfo paymentInfo { get; set; }
        public OrderStatusResponse orderStatus { get; set; }
    }

    public class OrderDetail
    {
        public string ActualDate { get; set; }
        public string ActualTime { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleTime { get; set; }
        public string OrderReadyDate { get; set; }
        public string OrderReadyTime { get; set; }
    }

    public class CustomerDetail
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
    }

    public class DriverDetail
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
    }

    public class ProviderDetail
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
    }

    public class PaymentInfo
    {
        public string PaymnetMethod { get; set; }
        public string PaymnetWindow { get; set; }
    }

    public class OrderStatusResponse
    {
        public string OrderStatus { get; set; }
        public string DeliverPickUp { get; set; }
    }

    public class ItemList
    {

    }
}
