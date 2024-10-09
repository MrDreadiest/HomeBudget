using HomeBudget.Api.Data;
using HomeBudget.Api.Entities;
using HomeBudget.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Api.Repositories
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly DataContext _context;

        public ExpenseCategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExpenseCategory>> GetExpenseCategoriesByBudgetIdAsync(string budgetId)
        {
            return await _context.ExpenseCategories
                .Include(e => e.Budget)
                .Where(e => e.BudgetId.Equals(budgetId)).ToListAsync();
        }

        public async Task<ExpenseCategory?> GetExpenseCategoryByIdAsync(string categoryId)
        {
            return await _context.ExpenseCategories.FindAsync(categoryId);
        }

        public async Task AddAsync(ExpenseCategory expenseCategory)
        {
            await _context.ExpenseCategories.AddAsync(expenseCategory);
        }

        public void Update(ExpenseCategory expenseCategory)
        {
            _context.ExpenseCategories.Update(expenseCategory);
        }

        public void Delete(ExpenseCategory expenseCategory)
        {
            _context.ExpenseCategories.Remove(expenseCategory);
        }
    }
}
