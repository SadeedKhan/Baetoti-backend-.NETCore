using System;

namespace Baetoti.Shared.Response.Transaction
{
    public class AllTransactions
    {
        public long TransactionID { get; set; }
        public long UserID { get; set; }
        public long OrderID { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionFrom { get; set; }
        public string TransactionTo { get; set; }
        public string TransactionFor { get; set; }
        public string TransactionStatus { get; set; }
        public string PaymentType { get; set; }
        public DateTime TransactionTime { get; set; }
    }
}
