using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KovsieCash_WebApp.Models
{
    public class Account
    {
        // Primary Key for the account
        [Key]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        // User ID of the account holder
        [Required]
        public string UserId { get; set; } // Foreign key for user

        // Other account-related properties can be added here
        public string AccountHolderName { get; set; } // Example property
        public decimal Balance { get; set; } // Example property

        // Navigation property for transactions related to this account
        public ICollection<Transaction> Transactions { get; set; }
    }
}
