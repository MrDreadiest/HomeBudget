using HomeBudget.Api.Entities;

namespace HomeBudget.Api.Repositories.Interfaces
{
    public interface IExpenseCategoryRepository : IRepository<ExpenseCategory>
    {
        Task<ExpenseCategory?> GetExpenseCategoryByIdAsync(string categoryId);
        Task<IEnumerable<ExpenseCategory>> GetExpenseCategoriesByBudgetIdAsync(string budgetId);
    }
}
