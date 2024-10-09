using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Api.UnitOfWork.Interfaces;
using HomeBudget.Common.EntityDTOs.Expense;

namespace HomeBudget.Api.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpenseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ExpenseCreateResponseModel?> CreateExpenseAsync(string userId, string budgetId, ExpenseCreateRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteExpense(string userId, string budgetId, string categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ExpenseGetResponseModel?> GetExpenseByIdsAsync(string userId, string budgetId, string expenseId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExpenseGetResponseModel>> GetExpensesByBudgetIdAsync(string userId, string budgetId)
        {
            throw new NotImplementedException();
        }

        public Task<ExpenseUpdateResponseModel?> UpdateExpenseAsync(string userId, string budgetId, string expenseId, ExpenseUpdateRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> HasAccessToBudgetAsync(string userId, string budgetId)
        {
            var budget = await _unitOfWork.Budgets.GetBudgetByIdAsync(budgetId);
            return budget != null && budget.Users.Any(u => u.Id.Equals(userId));
        }
    }
}
