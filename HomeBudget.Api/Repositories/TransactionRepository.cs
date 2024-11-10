using HomeBudget.Api.Data;
using HomeBudget.Api.Entities;
using HomeBudget.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Api.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataContext _context;

        public TransactionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByBudgetIdAsync(string budgetId)
        {
            return await _context.Transactions
                .Include(e => e.Budget)
                .Include(e => e.TransactionCategory)
                .Where(e => e.Budget.Id.Equals(budgetId))
                .ToListAsync();
        }

        public async Task<Transaction?> GetTransactionByIdAsync(string transactionId)
        {
            return await _context.Transactions.FindAsync(transactionId);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByBudgetIdInDateRangeAsync(
            string budgetId,
            DateTime? startDate,
            DateTime? endDate)
        {
            var query = _context.Transactions
                .Include(e => e.Budget)
                .Include(e => e.TransactionCategory)
                .Where(e => e.Budget.Id.Equals(budgetId));

            if (startDate.HasValue)
            {
                query = query.Where(e => e.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.Date <= endDate.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByBudgetIdAndCategoriesInDataRangeAsync(
            string budgetId,
            DateTime? startDate,
            DateTime? endDate,
            List<string> categoryIds)
        {
            var query = _context.Transactions
                .Include(t => t.Budget)
                .Include(t => t.TransactionCategory)
                .Where(t => t.Budget.Id.Equals(budgetId));

            if (categoryIds.Any())
            {
                query = query.Where(t => categoryIds.Contains(t.TransactionCategory.Id));
            }

            if (startDate.HasValue)
            {
                query = query.Where(e => e.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.Date <= endDate.Value);
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }

        public void Update(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
        }

        public void Delete(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
        }


    }
}
