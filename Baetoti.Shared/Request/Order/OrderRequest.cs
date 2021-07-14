using System;
using System.Collections.Generic;

namespace Baetoti.Shared.Request.Order
{
    public class OrderRequest
    {
        public long UserIID { get; set; }
        public string NotesForDriver { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime ExpectedDeliveryTime { get; set; }
        public DateTime ActualDeliveryTime { get; set; }
        public List<RequestedItemList> Items { get; set; }
    }

    public class RequestedItemList
    {
        public long ItemID { get; set; }
        public int Quantity { get; set; }
        public string Comments { get; set; }
    }

}
