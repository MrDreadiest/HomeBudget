using HomeBudget.App.Models;

namespace HomeBudget.App.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<bool> GetAllTransactionAsync(Budget budget);
        Task<List<Transaction>> GetTransactionInRangeAsync(string budgetId, DateTime startDate, DateTime endDate);
        Task<Transaction?> GetTransactionByIdAsync(string budgetId, string transactionId);
        Task<bool> CreateTransactionAsync(Budget budget, Transaction transaction);
        Task<bool> CreateTransactionsAsync(Budget budget, List<Transaction> transactions);
        Task<bool> UpdateTransactionAsync(Budget budget, Transaction transaction);
        Task<bool> DeleteTransactionAsync(Budget budget, Transaction transaction);
    }
}
