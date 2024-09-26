using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace KovsieCash_WebApp.Models
{
    public class Transaction
    {
        // Primary Key for the transaction
        [Key]
        public int TransactionId { get; set; }

        // Reference attribute for identifying a transaction
        [Required]
        [StringLength(50)]
        public string Reference { get; set; }

        // Date and time when the transaction occurred
        [Required]
        public DateTime DateTime { get; set; }

        // The amount involved in the transaction
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        // Type of the transaction: Deposit, Withdrawal, or Transfer
        [Required]
        public TransactionType Type { get; set; }

		// Ongoing balance on account
		[Required]
		[Column(TypeName = "decimal(18, 2)")]
		public decimal Balance { get; set; }

		// Foreign key to Account using AccountNumber
		[Required]
        [StringLength(20)]
        public string AccountNumber { get; set; } // Foreign key

        // Navigation property to the linked account
        [ForeignKey("AccountNumber")]
        public Account Account { get; set; }
    }
  
}
