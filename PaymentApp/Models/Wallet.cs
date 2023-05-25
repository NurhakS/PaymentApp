using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentApp.Models
{
    public partial class Wallet
    {
        public int WalletId { get; set; }
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
        public int? CustomerId { get; set; }
        public int Currency { get; set; }

        public virtual Currency CurrencyNavigation { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
