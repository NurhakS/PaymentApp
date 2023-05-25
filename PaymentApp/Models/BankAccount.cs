using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PaymentApp.Models
{
    public partial class BankAccount
    {
        public BankAccount()
        {
            Withdraws = new HashSet<Withdraw>();
        }
        
        public int BankAccountId { get; set; }
        [Required]
        [StringLength(34)]
        [MinLength(34 ,ErrorMessage = "Name length can't be less than 34.")]
        public string IbanCustomer { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public int CustomerId { get; set; }
        public int BankId { get; set; }

        public virtual Bank Bank { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Withdraw> Withdraws { get; set; }
    }
}
