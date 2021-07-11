using System;
using System.Collections.Generic;

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
        public decimal RevenueGain { get; set; }
        public decimal RevenueLoss { get; set; }
        public decimal AverageCheckValue { get; set; }
        public int FeedBackCollected { get; set; }
        public int OrderWithFullAdoptionAndCompliance { get; set; }
        public int OrdersDeliverUnder30Minutes { get; set; }
        public decimal AdoptionPercentage { get; set; }
        public decimal CompliancePercentage { get; set; }
        public decimal OrderRunRate { get; set; }
        public ProviderOrderStates ProviderOrder { get; set; }
        public DriverOrderStates DriverOrder { get; set; }
        public List<OrderStates> OrderList { get; set; }
    }

    public class ProviderOrderStates
    {
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Canceled { get; set; }
    }

    public class DriverOrderStates
    {
        public int Pending { get; set; }
        public int Delivered { get; set; }
        public int PickedUp { get; set; }
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
