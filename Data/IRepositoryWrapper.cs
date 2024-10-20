namespace KovsieCash_WebApp.Data
{
    public interface IRepositoryWrapper
    {
        IAccountRepository Accounts { get; }
        ITransactionRepository Transactions { get; }
        IApplicationUserRepository ApplicationUsers { get; }

        IAdviceRepository Advice {  get; }

        IBeneficiaryRepository Beneficiaries { get; }

        IReviewRepository Reviews { get; }

        INotificationRepository Notifications { get; }

        void Save(); // Method to save changes to the database
    }
}
