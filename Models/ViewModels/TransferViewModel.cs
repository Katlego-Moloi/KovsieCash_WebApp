using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KovsieCash_WebApp.Models.ViewModels
{
    public class TransferModel
    {
        [Required]
        [StringLength(20)]
        public string fromAccNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string toAccNumber { get; set; }

        // Reference attribute for identifying a transaction
        [Required]
        [StringLength(50)]
        public string Reference { get; set; }

        // The amount involved in the transaction
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }


    }
}

