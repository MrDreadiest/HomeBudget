using HomeBudget.App.Models;

namespace HomeBudget.App.Services.Interfaces
{
    public interface ITransactionCategoryService
    {
        Task<bool> GetAllTransactionCategoriesAsync(Budget budget);
        Task<TransactionCategory> GetTransactionCategoryByIdsAsync(string budgetId, string categoryId);
        Task<bool> CreateTransactionCategoryAsync(Budget budget, TransactionCategory transactionCategory);
        Task<bool> CreateTransactionCategoriesAsync(Budget budget, List<TransactionCategory> transactionCategories);
        Task<bool> UpdateTransactionCategoryAsync(Budget budget, TransactionCategory transactionCategory);
        Task<bool> DeleteTransactionCategoryAsync(Budget budget, TransactionCategory transactionCategory);
    }
}