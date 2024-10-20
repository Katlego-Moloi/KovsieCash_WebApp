using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KovsieCash_WebApp.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public string ReviewTitle { get; set; }

        public string ReviewDescription { get; set; }

        public int ReviewRating { get; set;}

        public DateTime ReviewDate { get; set; }

        // Navigation Properties
        // User ID of the Reviewee
        [Required]
        public string UserId { get; set; } // Foreign key for user

        [ForeignKey("UserId")]
        [Required]
        public ApplicationUser Reviewee { get; set; }

    }
}
