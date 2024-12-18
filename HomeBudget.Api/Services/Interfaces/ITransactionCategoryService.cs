﻿using HomeBudget.Common.EntityDTOs.TransactionCategory;

namespace HomeBudget.Api.Services.Interfaces
{
    public interface ITransactionCategoryService
    {
        Task<IEnumerable<TransactionCategoryGetResponseModel>> GetTransactionCategoriesByBudgetIdAsync(string userId, string budgetId);
        Task<IEnumerable<TransactionCategoryGetResponseModel>> GetTopAmountTransactionCategoriesByBudgetIdInDateRangeAsync(string userId, string budgetId, int count, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<TransactionCategoryGetResponseModel>> GetTopCountTransactionCategoriesByBudgetIdInDateRangeAsync(string userId, string budgetId, int count, DateTime? startDate, DateTime? endDate);
        Task<TransactionCategoryGetResponseModel?> GetTransactionCategoryByIdsAsync(string userId, string budgetId, string categoryId);
        Task<TransactionCategoryCreateResponseModel?> CreateTransactionCategoryAsync(string userId, string budgetId, TransactionCategoryCreateRequestModel requestModel);
        Task<IEnumerable<TransactionCategoryCreateResponseModel>> CreateTransactionCategoriesAsync(string userId, string budgetId, IEnumerable<TransactionCategoryCreateRequestModel> requestModels);
        Task<TransactionCategoryUpdateResponseModel?> UpdateTransactionCategoryAsync(string userId, string budgetId, string categoryId, TransactionCategoryUpdateRequestModel requestModel);
        Task<bool> DeleteTransactionCategory(string userId, string budgetId, string categoryId);
    }
}
