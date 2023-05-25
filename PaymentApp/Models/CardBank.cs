using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PaymentApp.Models
{
    public partial class CardBank
    {
        public CardBank()
        {
            DepositCards = new HashSet<DepositCard>();
        }

        public int CardBankId { get; set; }
        [Required]
        [StringLength(16)]
        [MinLength(16, ErrorMessage = "Name length can't be less than 34.")]
        public string CardNumber { get; set; }
        public DateTime ExpireDate { get; set; }
        [Required]
        [StringLength(3)]
        [MinLength(3, ErrorMessage = "Name length can't be less than 34.")]
        public string SecurityCode { get; set; }
        public int BankId { get; set; }
        public int? CustomerId { get; set; }
        [Required]
        public string CardName { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<DepositCard> DepositCards { get; set; }
    }
}
