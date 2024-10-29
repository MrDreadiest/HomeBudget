using HomeBudget.Api.Repositories.Interfaces;

namespace HomeBudget.Api.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IAddressRepository Addresses { get; }
        IBudgetRepository Budgets { get; }
        ITransactionCategoryRepository TransactionCategories { get; }
        ITransactionRepository Transactions { get; }

        Task<int> SaveChangesAsync();
    }
}
