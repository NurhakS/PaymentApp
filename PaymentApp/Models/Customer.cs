using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentApp.Models
{
    public partial class Customer
    {
        public Customer()
        {
            BankAccounts = new HashSet<BankAccount>();
            CardBanks = new HashSet<CardBank>();
            CardVirtuals = new HashSet<CardVirtual>();
            Wallets = new HashSet<Wallet>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPassword { get; set; }
        public string CustomerTel { get; set; }
        public string CustomerMail { get; set; }
        public string AccountNumber { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<CardBank> CardBanks { get; set; }
        public virtual ICollection<CardVirtual> CardVirtuals { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
