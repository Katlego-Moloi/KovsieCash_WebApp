namespace KovsieCash_WebApp.Data
{
    public interface IUnitOfWork
    {
        IAccountRepository Accounts { get; }
        ITransactionRepository Transactions { get; }

        void Save(); // Method to save changes to the database
    }
}
