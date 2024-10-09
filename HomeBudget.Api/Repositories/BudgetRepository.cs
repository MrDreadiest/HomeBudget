using HomeBudget.Api.Data;
using HomeBudget.Api.Entities;
using HomeBudget.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Api.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly DataContext _context;

        public BudgetRepository(DataContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(string userId)
        {
            return await _context.Budgets
                .Include(b => b.Users)
                .Where(b=>b.Users.Any(u=>u.Id.Equals(userId))).ToListAsync();
        }

        public async Task<Budget?> GetBudgetByIdAsync(string budgetId)
        {
            return await _context.Budgets
                .Include(b => b.Users)
                .FirstOrDefaultAsync(b => b.Id.Equals(budgetId));
        }

        public async Task AddAsync(Budget budget)
        {
            await _context.Budgets.AddAsync(budget);
        }

        public void Delete(Budget budget)
        {
            _context.Budgets.Remove(budget);
        }

        public void Update(Budget budget)
        {
            _context.Budgets.Update(budget);
        }
    }
}
