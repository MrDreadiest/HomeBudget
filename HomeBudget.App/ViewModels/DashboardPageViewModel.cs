using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Extensions;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels.HandViews;
using HomeBudget.App.ViewModels.Widgets;
using HomeBudget.App.Views;

namespace HomeBudget.App.ViewModels
{
    public partial class DashboardPageViewModel : BaseViewModel
    {
        [RelayCommand]
        public async Task ToggleVisibility()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                IsVisible = !IsVisible;
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

        //TODO : Aktualizacaj danych
        [RelayCommand]
        public async Task Refresh()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

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
        private CurrentBudgetContentViewModel currentBudgetVM;

        [ObservableProperty]
        private FastBalanceContentViewModel _fastBalanceVM;

        [ObservableProperty]
        private ShortcutsContentViewModel shortcutsVM;

        [ObservableProperty]
        private RegularTransactionsReminderContentViewModel regularTransactionsReminderVM;

        [ObservableProperty]
        private LastTransactionsReminderContentViewModel lastTransactionsReminderVM;

        private readonly IAppSettingsService _appSettingsService;
        private readonly IUserService _userService;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly ITransactionService _transactionService;


        public DashboardPageViewModel(IAppSettingsService appSettingsService, IUserService userService, IBudgetService budgetService, ITransactionService transactionService, ITransactionCategoryService transactionCategoryService)
        {
            Route = nameof(DashboardPageAndroidView);
            //TODO: zasoby
            Title = "Kokpit";
            IconUnicode = Icons.Home;

            _appSettingsService = appSettingsService;
            _userService = userService;
            _budgetService = budgetService;
            _transactionService = transactionService;
            _transactionCategoryService = transactionCategoryService;

            CurrentBudgetVM = new CurrentBudgetContentViewModel(_appSettingsService, _budgetService);
            FastBalanceVM = new FastBalanceContentViewModel();
            ShortcutsVM = new ShortcutsContentViewModel();
            RegularTransactionsReminderVM = new RegularTransactionsReminderContentViewModel();
            LastTransactionsReminderVM = new LastTransactionsReminderContentViewModel(_appSettingsService);
        }

        public async override Task OnAppearingAsync()
        {
            try
            {
                IsBusy = true;
                IsVisible = true;

                await Task.Delay(100);

                if (_budgetService.CurrentBudget.IsNullOrEmpty())
                {
                    await Shell.Current.GoToAsync($"//{nameof(BudgetsPageAndroidView)}");
                }
                else
                {
                    _ = Task.Run(() => FastBalanceVM.ReloadData());
                    _ = Task.Run(() => LastTransactionsReminderVM.Reload(
                        _budgetService.CurrentBudget.Transactions,
                        _budgetService.CurrentBudget.TransactionCategories
                    ));
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ResetView()
        {
            await FastBalanceVM.ReloadData();
            await LastTransactionsReminderVM.Reload(_budgetService.CurrentBudget.Transactions, _budgetService.CurrentBudget.TransactionCategories);
        }

        public async override Task OnDisappearingAsync()
        {
            IsVisible = false;
            await Task.CompletedTask;
        }
    }
}