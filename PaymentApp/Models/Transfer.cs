using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PaymentApp.Models
{
    public partial class Transfer
    {
        public int TransId { get; set; }
        [Required]
        public string AccountNumberGiver { get; set; }
        [Required]
        public string AccountNumberTaker { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public int Currency { get; set; }

        public virtual Currency CurrencyNavigation { get; set; }
    }
}
