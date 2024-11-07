using HomeBudget.Api.Entities;

namespace HomeBudget.Api.Repositories.Interfaces
{
    public interface ITransactionCategoryRepository : IRepository<TransactionCategory>
    {
        Task<TransactionCategory?> GetTransactionCategoryByIdAsync(string categoryId);
        Task<IEnumerable<TransactionCategory>> GetTransactionCategoriesByBudgetIdAsync(string budgetId);
    }
}
