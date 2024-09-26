using KovsieCash_WebApp.Models;
using System.Collections.Generic;

namespace KovsieCash_WebApp.Data
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
		protected AppDbContext _appDbContext;
        public TransactionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
			_appDbContext = appDbContext;
        }

		public void AddTransaction(string accountNumber, string reference, TransactionType transactionType, decimal amount)
		{


			Create(new Transaction
			{
				AccountNumber = accountNumber,
				Amount = transactionType == TransactionType.Deposit? amount:Decimal.Negate(amount),
				Reference = reference == "" ? reference : GenerateReference(transactionType),
				Type = transactionType,
				DateTime = DateTime.Now,
				Balance = UpdateAccountBalance(accountNumber, transactionType, amount)

			});

		}

		public void AddTransaction(string accNumFrom, string accNumTo, decimal amount, string reference)
		{
			Create(new Transaction
			{
				AccountNumber = accNumFrom,
				Amount = -amount,
				Reference = reference == "" ? reference : GenerateReference(TransactionType.Transfer),
				Type = TransactionType.Transfer,
				DateTime = DateTime.Now,
				Balance = UpdateAccountBalance(accNumFrom, TransactionType.Withdrawal, amount)
			});

			Create(new Transaction
			{
				AccountNumber = accNumTo,
				Amount = amount,
				Reference = reference == "" ? reference : GenerateReference(TransactionType.Transfer),
				Type = TransactionType.Transfer,
				DateTime = DateTime.Now,
				Balance = UpdateAccountBalance(accNumTo, TransactionType.Deposit, amount)
			});

			
		}

		public decimal UpdateAccountBalance(string accountNumber, TransactionType transactionType, decimal amount)
		{
			Account account = _appDbContext.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
			account.Balance = transactionType == TransactionType.Deposit ? account.Balance + amount : account.Balance - amount;
			_appDbContext.Accounts.Update(account);

			return account.Balance;
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

		public static string GenerateReference(TransactionType transactionType)
		{
			// Get the current date and time in the format "yyyyMMddHHmmss" (year, month, day, hour, minute, second)
			string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

			// Generate a random 4-digit number for extra uniqueness
			Random random = new Random();
			int randomDigits = random.Next(1000, 9999);

			// Determine the abbreviation for the transaction type
			string transactionAbbreviation = transactionType switch
			{
				TransactionType.Transfer => "TRF",
				TransactionType.Deposit => "DEP",
				TransactionType.Withdrawal => "WTH",
				_ => "UNK" 
			};

			// Combine the components into a single reference string
			return $"{transactionAbbreviation}-{timestamp}-{randomDigits}";
		}


	}
}
