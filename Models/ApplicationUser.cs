using Microsoft.AspNetCore.Identity;

namespace KovsieCash_WebApp.Models
{
    namespace KovsieCash_WebApp.Models
    {
        public class ApplicationUser : IdentityUser
        {
            //public string UserType { get; set; } // E.g., "Admin", "Consultant", "Advisor", "Customer"

            // Navigation property for accounts linked to this user
            public ICollection<Account> Accounts { get; set; }
        }
    }
}

