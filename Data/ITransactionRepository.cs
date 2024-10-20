﻿using KovsieCash_WebApp.Models;
using System.Collections.Generic;

namespace KovsieCash_WebApp.Data
{
    public interface ITransactionRepository : IRepositoryBase<Transaction>
    {
        IEnumerable<Transaction> GetTransactionsByAccountNumber(string accountNumber);
        IEnumerable<Transaction> GetTransactionsByUserId(string userId);

        void AddTransaction(string accountNumber, string reference, TransactionType transactionType, double amount);

		void AddTransaction(string accNumFrom, string accNumTo, double amount, string reference);
	}
}
