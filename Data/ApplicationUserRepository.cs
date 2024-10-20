using KovsieCash_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Collections.Generic;

namespace KovsieCash_WebApp.Data
{
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public IEnumerable<ApplicationUser> GetUserById(string userId)
        {
            return FindByCondition(user => user.Id == userId);
        }

        public IEnumerable<ApplicationUser> GetUsersWithAccounts()
        {
            return _appDbContext.ApplicationUsers.Include(u => u.Accounts);
        }

        public ApplicationUser GetUserWithAccounts(string userId)
        {
            return _appDbContext.ApplicationUsers.Include(u => u.Accounts).FirstOrDefault(u => u.Id == userId);
        }

    }
}
