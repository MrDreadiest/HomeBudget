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
                .Include(e => e.Transactions)
                .OrderByDescending(e => e.Transactions.Count)
                .Where(e => e.BudgetId.Equals(budgetId)).ToListAsync();
        }

        public async Task<IEnumerable<TransactionCategory>> GetTopAmountTransactionCategoriesByBudgetIdInDateRangeAsync(string budgetId, int count, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.TransactionCategories
                .Include(e => e.Budget)
                .Include(e => e.Transactions)
                .Where(e => e.BudgetId.Equals(budgetId));

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(e => e.Transactions.Any(t => t.Date >= startDate.Value && t.Date <= endDate.Value));
            }

            query = query.OrderByDescending(e => e.Transactions
            .Where(t => (!startDate.HasValue || t.Date >= startDate.Value) && (!endDate.HasValue || t.Date <= endDate.Value))
            .Sum(t => t.TotalAmount));

            return count > 0 ? await query.Take(count).ToListAsync() : await query.ToListAsync();
        }

        public async Task<IEnumerable<TransactionCategory>> GetTopCountTransactionCategoriesByBudgetIdInDateRangeAsync(string budgetId, int count, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.TransactionCategories
                .Include(e => e.Budget)
                .Include(e => e.Transactions)
                .Where(e => e.BudgetId.Equals(budgetId));

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(e => e.Transactions.Any(t => t.Date >= startDate.Value && t.Date <= endDate.Value));
            }

            query = query.OrderByDescending(e => e.Transactions
            .Where(t => (!startDate.HasValue || t.Date >= startDate.Value) && (!endDate.HasValue || t.Date <= endDate.Value))
            .Count());

            return count > 0 ? await query.Take(count).ToListAsync() : await query.ToListAsync();
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
