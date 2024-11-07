using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.Views;

namespace HomeBudget.App.ViewModels.ContentViewModels.UniversalControls
{
    public partial class SelectBudgetContentViewModel : ObservableObject
    {
        [RelayCommand]
        public async Task SelectBudget()
        {
            _budgetService.SelectBudgetAsCurrent(Budget.Id);
            await Shell.Current.GoToAsync($"//{nameof(DashboardPageAndroidView)}");
        }

        [ObservableProperty]
        private Budget _budget;

        private readonly IBudgetService _budgetService;

        public SelectBudgetContentViewModel(IBudgetService budgetService, Budget budget)
        {
            _budgetService = budgetService;
            _budget = budget;
        }
    }
}
