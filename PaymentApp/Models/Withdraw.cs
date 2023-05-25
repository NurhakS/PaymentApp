using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentApp.Models
{
    public partial class Withdraw
    {
        public int WithdrawId { get; set; }
        public string IbanCustomer { get; set; }
        public double Value { get; set; }
        public string Currency { get; set; }
        public int BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }
    }
}
