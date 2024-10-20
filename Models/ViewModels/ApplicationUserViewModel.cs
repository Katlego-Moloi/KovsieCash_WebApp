using System.Security;

namespace KovsieCash_WebApp.Models.ViewModels
{
    public class ApplicationUserModel
    {
        public String Id { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string UserType { get; set; }

        public IEnumerable<Account> Accounts { get; set; }
    }
}
