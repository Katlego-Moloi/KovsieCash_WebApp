using KovsieCash_WebApp.Models;
using System.Collections.Generic;

namespace KovsieCash_WebApp.Data
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public IEnumerable<Account> GetAccountsByUserId(string userId)
        {
            return FindByCondition(account => account.UserId == userId).ToList();
        }

    }
}
