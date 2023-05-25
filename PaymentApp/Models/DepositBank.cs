using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentApp.Models
{
    public partial class DepositBank
    {
        public DepositBank()
        {
            Deposits = new HashSet<Deposit>();
        }

        public int DepositBankId { get; set; }
        public string AccountNumber { get; set; }
        public int BankId { get; set; }
        public double Value { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual ICollection<Deposit> Deposits { get; set; }
    }
}
