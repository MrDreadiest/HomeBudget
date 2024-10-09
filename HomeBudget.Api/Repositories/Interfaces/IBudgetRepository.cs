using HomeBudget.Api.Entities;

namespace HomeBudget.Api.Repositories.Interfaces
{
    public interface IBudgetRepository : IRepository<Budget>
    {
        Task<Budget?> GetBudgetByIdAsync(string budgetId);
        Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(string userId);
    }
}
