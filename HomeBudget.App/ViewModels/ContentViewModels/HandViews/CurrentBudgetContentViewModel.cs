using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.Views.ContentViews.FullViews;
using HomeBudget.App.Views.ContentViews.HandViews;

namespace HomeBudget.App.ViewModels.ContentViewModels.HandViews
{
    public partial class CurrentBudgetContentViewModel : HandViewBaseModel, IHandViewBaseModel
    {
        [RelayCommand]
        public async Task GoToFullView()
        {
            if (IsBusy)
                return;
            try
            {
                await Shell.Current.GoToAsync($"//{FullViewRoute}");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        [ObservableProperty]
        private Budget _budget;

        private readonly IAppSettingsService _appSettingsService;
        private readonly IBudgetService _budgetService;

        public CurrentBudgetContentViewModel(IAppSettingsService appSettingsService, IBudgetService budgetService)
        {
            // TODO: Przeniesienie do zasobów
            Title = "Aktualny budżet";
            Route = nameof(CurrentBudgetContentView);
            FullViewRoute = nameof(ManageCurrentBudgetAndroidPageView);

            Budget = budgetService.CurrentBudget;

            _budgetService = budgetService;
            _appSettingsService = appSettingsService;

            _budgetService.CurrentBudgetChanged += BudgetService_CurrentBudgetChanged;
        }

        private void BudgetService_CurrentBudgetChanged(object? sender, EventArgs e)
        {
            Budget = _budgetService.CurrentBudget;
        }
    }
}
