using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentApp.Models
{
    public partial class DepositCard
    {
        public DepositCard()
        {
            Deposits = new HashSet<Deposit>();
        }

        public int DepositCardId { get; set; }
        public int CardId { get; set; }
        public string AccountNumber { get; set; }
        public double Value { get; set; }

        public virtual CardBank Card { get; set; }
        public virtual CardVirtual CardNavigation { get; set; }
        public virtual ICollection<Deposit> Deposits { get; set; }
    }
}
