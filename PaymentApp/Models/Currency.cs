using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentApp.Models
{
    public partial class Currency
    {
        public Currency()
        {
            Deposits = new HashSet<Deposit>();
            Transfers = new HashSet<Transfer>();
            Wallets = new HashSet<Wallet>();
        }

        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public double CurrencyDollarValue { get; set; }

        public virtual ICollection<Deposit> Deposits { get; set; }
        public virtual ICollection<Transfer> Transfers { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
