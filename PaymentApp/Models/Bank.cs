using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentApp.Models
{
    public partial class Bank
    {
        public Bank()
        {
            BankAccounts = new HashSet<BankAccount>();
            CardBanks = new HashSet<CardBank>();
            DepositBanks = new HashSet<DepositBank>();
        }

        public int BankId { get; set; }
        public string BankName { get; set; }
        public string Iban { get; set; }

        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<CardBank> CardBanks { get; set; }
        public virtual ICollection<DepositBank> DepositBanks { get; set; }
    }
}
