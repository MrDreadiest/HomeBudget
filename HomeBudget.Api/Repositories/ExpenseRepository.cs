using HomeBudget.Api.Data;
using HomeBudget.Api.Entities;
using HomeBudget.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Api.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly DataContext _context;

        public ExpenseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetExpensesByBudgetIdAsync(string budgetId)
        {
            return await _context.Expenses
                .Include(e => e.Budget)
                .Where(e => e.Budget.Id.Equals(budgetId))
                .ToListAsync();
        }

        public async Task<Expense?> GetExpenseByIdAsync(string expenseId)
        {
            return await _context.Expenses.FindAsync(expenseId);
        }

        public async Task AddAsync(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
        }

        public void Update(Expense expense)
        {
            _context.Expenses.Update(expense);
        }

        public void Delete(Expense expense)
        {
            _context.Expenses.Remove(expense);
        }
    }
}
