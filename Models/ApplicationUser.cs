using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace KovsieCash_WebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicture {  get; set; } = "default.webp";

        // Navigation property for accounts linked to this user
        public List<Account> Accounts { get; set; }

        public List<Notification> Notifications { get; set; }

        public List<Review> Reviews { get; set; }

        public List<Beneficiary> Beneficiaries { get; set; }

        // Advice received by this user (as Advisee)
        public List<Advice> AdviceReceived { get; set; }

        // Advice given by this user (as Adviser)
        public List<Advice> AdviceGiven { get; set; }
    }
}
