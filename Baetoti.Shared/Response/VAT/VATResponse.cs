using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Response.VAT
{
    public class VATResponse
    {
        public long ID { get; set; }

        public decimal VATTax { get; set; }

        public int UserTypeID { get; set; }

    }
}
