using HomeBudget.Common.EntityDTOs.Expense;

namespace HomeBudget.Api.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseGetResponseModel>> GetExpensesByBudgetIdAsync(string userId, string budgetId);
        Task<ExpenseGetResponseModel?> GetExpenseByIdsAsync(string userId, string budgetId, string expenseId);
        Task<ExpenseCreateResponseModel?> CreateExpenseAsync(string userId, string budgetId, ExpenseCreateRequestModel requestModel);
        Task<ExpenseUpdateResponseModel?> UpdateExpenseAsync(string userId, string budgetId, string expenseId, ExpenseUpdateRequestModel requestModel );
        Task<bool> DeleteExpense(string userId, string budgetId, string categoryId);
    }
}
