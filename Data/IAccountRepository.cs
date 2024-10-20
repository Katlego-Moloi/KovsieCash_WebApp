using KovsieCash_WebApp.Models;
using System.Collections.Generic;

namespace KovsieCash_WebApp.Data
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        IEnumerable<Account> GetAccountsByUserId(string userId);

		IEnumerable<Account> GetAccountsByUserWithHistory(string userId);

		public Account GetAccountWithHistory(string accountNumber);
    }
}
