using KovsieCash_WebApp.Models;
using System.Collections.Generic;

namespace KovsieCash_WebApp.Data
{
    public interface IApplicationUserRepository : IRepositoryBase<ApplicationUser>
    {
        IEnumerable<ApplicationUser> GetUserById(string userId);

        IEnumerable<ApplicationUser> GetUsersWithAccounts();

        public ApplicationUser GetUserWithAccounts(string userId);
    }
}
