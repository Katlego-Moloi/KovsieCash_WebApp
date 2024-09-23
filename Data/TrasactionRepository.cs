using KovsieCash_WebApp.Models;
using System.Collections.Generic;

namespace KovsieCash_WebApp.Data
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public IEnumerable<Transaction> GetTransactionsByAccountNumber(string accountNumber)
        {
            return FindByCondition(transaction => transaction.AccountNumber == accountNumber).ToList();
        }

        public IEnumerable<Transaction> GetTransactionsByUserId(string userId)
        {
            var accounts = _appDbContext.Set<Account>().Where(account => account.UserId == userId).Select(account => account.AccountNumber).ToList();
            return FindByCondition(transaction => accounts.Contains(transaction.AccountNumber)).ToList();
        }

    }
}
