﻿using HomeBudget.App.Models;

namespace HomeBudget.App.Services.Interfaces
{
    public interface IBudgetService
    {
        event EventHandler CurrentBudgetChanged;
        event EventHandler BudgetsChanged;

        IReadOnlyList<Budget> Budgets { get; }
        Budget CurrentBudget { get; set; }

        bool SelectBudgetAsCurrent(string id);
        Task TryGetLastBudgetAsCurrentAsync();

        Task<bool> GetAllBudgetsAsync();
        Task<Budget?> GetBudgetByIdAsync(string budgetId);
        Task<bool> CreateBudgetAsync(Budget budget);
        Task<bool> UpdateBudgetAsync(Budget budget);
        Task<bool> DeleteBudgetAsync(string budgetId);

    }
}
