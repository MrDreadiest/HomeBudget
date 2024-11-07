using HomeBudget.Api.Data;
using HomeBudget.Api.Entities;
using HomeBudget.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Api.Repositories
{
    public class TransactionCategoryRepository : ITransactionCategoryRepository
    {
        private readonly DataContext _context;

        public TransactionCategoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransactionCategory>> GetTransactionCategoriesByBudgetIdAsync(string budgetId)
        {
            return await _context.TransactionCategories
                .Include(e => e.Budget)
                .Where(e => e.BudgetId.Equals(budgetId)).ToListAsync();
        }

        public async Task<TransactionCategory?> GetTransactionCategoryByIdAsync(string categoryId)
        {
            return await _context.TransactionCategories.FindAsync(categoryId);
        }

        public async Task AddAsync(TransactionCategory transactionCategory)
        {
            await _context.TransactionCategories.AddAsync(transactionCategory);
        }

        public void Update(TransactionCategory transactionCategory)
        {
            _context.TransactionCategories.Update(transactionCategory);
        }

        public void Delete(TransactionCategory transactionCategory)
        {
            _context.TransactionCategories.Remove(transactionCategory);
        }
    }
}
