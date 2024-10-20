using KovsieCash_WebApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KovsieCash_WebApp.Models
{
    public class Account
    {
        // Primary Key for the account
        [Key]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        public string AccountName { get; set; } 
        public double Balance { get; set; } 

        // Navigation property for transactions related to this account
        public ICollection<Transaction> Transactions { get; set; }

        // User ID of the account holder
        [Required]
        public string UserId { get; set; } // Foreign key for user

        // Navigation property to the linked account
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
