﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Response.Invoice
{
    public class InvoiceResponse
    {
        public long OrderId { get; set; }

        public DateTime? OrderDate { get; set; }

        public string ProviderName { get; set; }
        public string ProviderAddress { get; set; }

        public string DriverName { get; set; }
        public string DriverAddress { get; set; }

        public string BuyerName { get; set; }
        public string BuyerAddress { get; set; }

        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }

        public decimal? DeliveryCharges { get; set; }

        public decimal? AmountPayable { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }
    }

    public class OrderDetails
    {
        public long? ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string PickUpFrom { get; set; }
        public DateTime? PickUpTime { get; set; }
        public string DeliveredTo { get; set; }
        public DateTime? DeliveryTime { get; set; }
    }
}
