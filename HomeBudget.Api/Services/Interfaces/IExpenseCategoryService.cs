using HomeBudget.Common.EntityDTOs.ExpenseCategory;

namespace HomeBudget.Api.Services.Interfaces
{
    public interface IExpenseCategoryService
    {
        Task<IEnumerable<ExpenseCategoryGetResponseModel>> GetExpenseCategoriesByBudgetIdAsync(string userId, string budgetId);
        Task<ExpenseCategoryGetResponseModel?> GetExpenseCategoryByIdsAsync(string userId, string budgetId, string categoryId);
        Task<ExpenseCategoryCreateResponseModel?> CreateExpenseCategoriesAsync(string userId, string budgetId ,ExpenseCategoryCreateRequestModel requestModel);
        Task<ExpenseCategoryUpdateResponseModel?> UpdateExpenseCategoriesAsync(string userId, string budgetId, string categoryId, ExpenseCategoryUpdateRequestModel requestModel);
        Task<bool> DeleteExpenseCategory(string userId, string budgetId, string categoryId);
    }
}
