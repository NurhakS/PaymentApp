using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentApp.Models
{
    public partial class CardVirtual
    {
        public CardVirtual()
        {
            DepositCards = new HashSet<DepositCard>();
        }

        public int CardIdVirtual { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpireDate { get; set; }
        public string SecurityCode { get; set; }
        public string NameCard { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<DepositCard> DepositCards { get; set; }
    }
}
