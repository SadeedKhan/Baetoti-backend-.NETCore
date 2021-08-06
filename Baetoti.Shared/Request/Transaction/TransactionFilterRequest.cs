using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Request.Transaction
{
    public class TransactionFilterRequest
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
