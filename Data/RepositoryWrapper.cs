using System;

namespace KovsieCash_WebApp.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppDbContext _appDbContext;
        private IAccountRepository _accounts;
        private ITransactionRepository _transactions;
        private IApplicationUserRepository _applicationUsers;
        private IReviewRepository _reviews;
        private IBeneficiaryRepository _beneficicaries;
        private IAdviceRepository _advice;
        private INotificationRepository _notifications;

        public RepositoryWrapper(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IApplicationUserRepository ApplicationUsers
        {
            get 
            {
                return _applicationUsers ??= new ApplicationUserRepository(_appDbContext);
            }
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

        public IAdviceRepository Advice
        {
            get
            {
                return _advice ??= new AdviceRepository(_appDbContext);
            }
        }

        public IBeneficiaryRepository Beneficiaries
        {
            get
            {
                return _beneficicaries ??= new BeneficiaryRepository(_appDbContext);
            }
        }

        public IReviewRepository Reviews
        {
            get
            {
                return _reviews ??= new ReviewRepository(_appDbContext);
            }
        }

        public INotificationRepository Notifications
        {
            get 
            {
                return _notifications ??= new NotificationRepository(_appDbContext);
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
