using HomeBudget.Api.Entities;

namespace HomeBudget.Api.Repositories.Interfaces
{
    public interface ITransactionCategoryRepository : IRepository<TransactionCategory>
    {
        Task<TransactionCategory?> GetTransactionCategoryByIdAsync(string categoryId);
        Task<IEnumerable<TransactionCategory>> GetTransactionCategoriesByBudgetIdAsync(string budgetId);
        Task<IEnumerable<TransactionCategory>> GetTopAmountTransactionCategoriesByBudgetIdInDateRangeAsync(string budgetId, int count, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<TransactionCategory>> GetTopCountTransactionCategoriesByBudgetIdInDateRangeAsync(string budgetId, int count, DateTime? startDate, DateTime? endDate);
    }
}
