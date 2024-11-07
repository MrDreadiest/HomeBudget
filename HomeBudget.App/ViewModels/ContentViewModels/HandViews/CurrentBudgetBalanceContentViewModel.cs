using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.Views.ContentViews.FullViews;
using HomeBudget.App.Views.ContentViews.HandViews;

namespace HomeBudget.App.ViewModels.ContentViewModels.HandViews
{
    public partial class CurrentBudgetBalanceContentViewModel : HandViewBaseModel, IHandViewBaseModel
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
        }




        internal async Task Reload(List<Transaction> transactions, List<TransactionCategory> transactionCategories)
        {
            await Task.CompletedTask;
        }

        private readonly IAppSettingsService _appSettingsService;

        public CurrentBudgetBalanceContentViewModel(IAppSettingsService appSettingsService)
        {
            // TODO: Przeniesienie do zasobów
            Title = "Bilans";
            Route = nameof(CurrentBudgetBalanceContentView);
            FullViewRoute = nameof(ManageCurrentBudgetBalanceAndroidPageView);

            _appSettingsService = appSettingsService;

        }
    }
}
