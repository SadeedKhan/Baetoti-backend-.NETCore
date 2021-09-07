using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Response.Commission
{
    public class CommissionResponse
    {
        public long ID { get; set; }

        public decimal Commission { get; set; }

        public int UserTypeID { get; set; }

    }
}
