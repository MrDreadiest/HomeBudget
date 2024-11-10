using HomeBudget.Api.Entities;

namespace HomeBudget.Api.Repositories.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<Transaction?> GetTransactionByIdAsync(string transactionId);
        Task<IEnumerable<Transaction>> GetTransactionsByBudgetIdAsync(string budgetId);
        Task<IEnumerable<Transaction>> GetTransactionsByBudgetIdInDateRangeAsync(string budgetId, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<Transaction>> GetTransactionsByBudgetIdAndCategoriesInDataRangeAsync(string budgetId, DateTime? startDate, DateTime? endDate, List<string> categoryIds);
    }
}
