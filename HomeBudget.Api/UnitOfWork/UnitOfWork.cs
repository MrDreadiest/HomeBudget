using HomeBudget.Api.Data;
using HomeBudget.Api.Repositories;
using HomeBudget.Api.Repositories.Interfaces;
using HomeBudget.Api.UnitOfWork.Interfaces;

namespace HomeBudget.Api.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public IUserRepository Users { get; }
        public IAddressRepository Addresses { get; }
        public IBudgetRepository Budgets { get; }
        public ITransactionCategoryRepository TransactionCategories { get; }
        public ITransactionRepository Transactions { get; }

        public UnitOfWork(DataContext context)
        {
            _context = context;
            
            Users = new UserRepository(_context);
            Addresses = new AddressRepository(_context);
            Budgets = new BudgetRepository(_context);
            TransactionCategories = new TransactionCategoryRepository(_context);
            Transactions = new TransactionRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
