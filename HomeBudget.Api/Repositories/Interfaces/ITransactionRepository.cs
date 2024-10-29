﻿using HomeBudget.Api.Entities;

namespace HomeBudget.Api.Repositories.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<Transaction?> GetTransactionByIdAsync(string transactionId);
        Task<IEnumerable<Transaction>> GetTransactionsByBudgetIdAsync(string budgetId);
    }
}
