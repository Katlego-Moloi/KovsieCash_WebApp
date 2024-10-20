using KovsieCash_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
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

		public IEnumerable<Account> GetAccountsByUserWithHistory(string userId)
		{
			return _appDbContext.Accounts.Where(a => a.UserId == userId).Include(a => a.Transactions);
		}

		public Account GetAccountWithHistory(string accountNumber)
        {
            return _appDbContext.Accounts.Where(a => a.AccountNumber == accountNumber).Include(a => a.Transactions).FirstOrDefault();
        }
    }
}
