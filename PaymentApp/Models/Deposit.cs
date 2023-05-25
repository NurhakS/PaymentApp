using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentApp.Models
{
    public partial class Deposit
    {
        public int DepositId { get; set; }
        public string AccountNumber { get; set; }
        public double Value { get; set; }
        public int DepositTypeId { get; set; }
        public int? Curency { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual Currency CurencyNavigation { get; set; }
        public virtual DepositBank DepositType { get; set; }
        public virtual DepositCard DepositTypeNavigation { get; set; }
    }
}
