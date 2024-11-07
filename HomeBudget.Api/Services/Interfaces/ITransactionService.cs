using HomeBudget.Common.EntityDTOs.Transaction;

namespace HomeBudget.Api.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionGetResponseModel>> GetTransactionsByBudgetIdAsync(string userId, string budgetId);
        Task<IEnumerable<TransactionGetResponseModel>> GetTransactionsByBudgetIdInDateRangeAsync(string userId, string budgetId, DateTime? startDate, DateTime? endDate);
        Task<TransactionGetResponseModel?> GetTransactionByIdsAsync(string userId, string budgetId, string transactionId);
        Task<TransactionCreateResponseModel?> CreateTransactionAsync(string userId, string budgetId, TransactionCreateRequestModel requestModel);
        Task<IEnumerable<TransactionCreateResponseModel>> CreateTransactionsAsync(string userId, string budgetId, IEnumerable<TransactionCreateRequestModel> requestModel);
        Task<TransactionUpdateResponseModel?> UpdateTransactionAsync(string userId, string budgetId, string transactionId, TransactionUpdateRequestModel requestModel);
        Task<bool> DeleteTransaction(string userId, string budgetId, string categoryId);
    }
}
