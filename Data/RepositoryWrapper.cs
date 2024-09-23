using System;

namespace KovsieCash_WebApp.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _appDbContext;
        private IAccountRepository _accounts;
        private ITransactionRepository _transactions;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IAccountRepository Accounts
        {
            get
            {
                return _accounts ??= new AccountRepository(_appDbContext);
            }
        }

        public ITransactionRepository Transactions
        {
            get
            {
                return _transactions ??= new TransactionRepository(_appDbContext);
            }
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
