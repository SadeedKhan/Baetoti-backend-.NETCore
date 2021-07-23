﻿using Baetoti.Core.Entites.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baetoti.Core.Entites
{
    [Table("Transaction", Schema = "baetoti")]
    public partial class Transaction : BaseEntity
    {
        public long UserID { get; set; }
        public long OrderID { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public int TransactionType { get; set; }
        public DateTime TransactionTime { get; set; }
    }
}
