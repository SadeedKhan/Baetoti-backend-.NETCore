using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Response.Order
{
    public class OrderResponse
    {
        public int Completed { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int InProgress { get; set; }
        public int Ready { get; set; }
        public int PickedUp { get; set; }
        public int Delivered { get; set; }
        public int Complaints { get; set; }
        public int CancelledByDriver { get; set; }
        public int CancelledByProvider { get; set; }
        public int CancelledByCustomer { get; set; }
        public decimal AverageRating { get; set; }
        public ProviderOrderStates ProviderOrder { get; set; }
        public DriverOrderStates DriverOrder { get; set; }
        public List<OrderStates> orderList { get; set; }
    }

    public class ProviderOrderStates
    {
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Canceled { get; set; }
    }

    public class DriverOrderStates
    {
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Canceled { get; set; }
    }

    public class OrderStates
    {
        public long OrderID { get; set; }
        public string Buyer { get; set; }
        public string Provider { get; set; }
        public string Driver { get; set; }
        public int OrderAmount { get; set; }
        public string PaymentType { get; set; }
        public DateTime Date { get; set; }
        public string ScheduleFor { get; set; }
        public string DeliverOrPickup { get; set; }
        public string OrderStatus { get; set; }
    }

}
