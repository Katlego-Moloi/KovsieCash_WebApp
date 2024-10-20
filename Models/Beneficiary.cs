using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KovsieCash_WebApp.Models
{
    public class Beneficiary
    {
        [Key]
        public int BeneficiaryId { get; set; }

        public string BeneficiaryName { get; set; }

        public string AccountNumbers { get; set; }

        // Navigation Properties
        // User ID of the Reviewee
        [Required]
        public string UserId { get; set; } // Foreign key for user

        [ForeignKey("UserId")]
        [Required]
        public ApplicationUser User { get; set; }

    }
}
