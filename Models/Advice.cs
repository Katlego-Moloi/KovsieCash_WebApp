using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KovsieCash_WebApp.Models
{
    public class Advice
    {
        [Key]
        public int AdviceID { get; set; }

        public string AdviceTitle { get; set; }

        public string AdviceDescription { get; set; }

        public DateTime AdviceDate { get; set; }

        // Foreign Key and Navigation Property for Adviser (the person giving the advice)
        [Required]
        public string AdviserId { get; set; }

        [ForeignKey("AdviserId")]
        public ApplicationUser Adviser { get; set; }

        // Foreign Key and Navigation Property for Advisee (the person receiving the advice)
        [Required]
        public string AdviseeId { get; set; }

        [ForeignKey("AdviseeId")]
        public ApplicationUser Advisee { get; set; }
    }


}
