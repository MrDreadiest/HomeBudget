using HomeBudget.Api.Entities;

namespace HomeBudget.Api.Repositories.Interfaces
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        Task<Expense?> GetExpenseByIdAsync(string expenseId);
        Task<IEnumerable<Expense>> GetExpensesByBudgetIdAsync(string budgetId);
    }
}
