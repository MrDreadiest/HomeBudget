using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Models.AccountSetup;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels.AccountSetup;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;
using HomeBudget.App.Views;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels
{
    public partial class UserAccountSetupPageViewModel : BaseViewModel
    {
        [RelayCommand]
        public async Task Next()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var currentVM = AccountSetupCarouselVMs[CarouselPosition];
                if (currentVM.CanGoNext())
                {
                    if (CarouselPosition < AccountSetupCarouselVMs.Count - 1)
                    {
                        CarouselPosition++;
                    }
                    else
                    {
                        //TODO: zasoby
                        var isConfirmed = await AskForConfirmation("Czy na pewno chcesz zakończyć konfigurację konta?");
                        if (!isConfirmed)
                            return;

                        bool result = await FinalizeUserAccountSetup();
                        if (result)
                        {
                            await Shell.Current.GoToAsync($"//{nameof(BudgetsPageAndroidView)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Handle exception (maybe log it)
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task Back()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                if (CarouselPosition > 0)
                {
                    CarouselPosition--;
                }
                else
                {
                    //TODO: zasoby
                    var isConfirmed = await AskForConfirmation("Czy na pewno chcesz opuścić konfigurację konta?");
                    if (!isConfirmed)
                        return;

                    await _authenticationService.LogoutAsync();
                    await Shell.Current.GoToAsync($"//{nameof(MainPageAndroidView)}");
                }
            }
            catch (Exception ex)
            {
                //TODO: Handle exception (maybe log it)
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ObservableCollection<AccountSetupCarouselItemViewModelBase> AccountSetupCarouselVMs { get; }

        public StateIconIndicatorContentViewModel StateIndicatorVM { get; }

        [ObservableProperty]
        private int _carouselPosition;

        private readonly IAuthenticationService _authenticationService;
        private readonly IAppSettingsService _appSettingsService;
        private readonly IUserService _userService;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;

        public UserAccountSetupPageViewModel(IUserService userService, IBudgetService budgetService, ITransactionCategoryService transactionCategoryService, IAuthenticationService authenticationService, IAppSettingsService appSettingsService)
        {
            _userService = userService;
            _budgetService = budgetService;
            _transactionCategoryService = transactionCategoryService;
            _authenticationService = authenticationService;
            _appSettingsService = appSettingsService;

            StateIndicatorVM = new StateIconIndicatorContentViewModel(
                Enum.GetValues(typeof(AccountSetupContentViewsType)).Cast<AccountSetupContentViewsType>()
                .Select(t => new StateIconItem(t.GetDescriptions(), t, t.GetIcon()))
                .ToList());
            StateIndicatorVM.ProcessStateChanged += StateIndicatorVM_ProcessStateChanged;

            AccountSetupCarouselVMs = new ObservableCollection<AccountSetupCarouselItemViewModelBase>()
            {
                new AccountSetupUserContentViewModel(),
                new AccountSetupBudgetContentViewModel(),
                new AccountSetupCategoriesContentViewModel(),
            };
        }

        private void StateIndicatorVM_ProcessStateChanged(object? sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        public async override Task OnAppearingAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                IsVisible = true;


                await Task.Run(async () =>
                {
                    var tasks = AccountSetupCarouselVMs.Select(vm => vm.ResetView()).ToList();
                    await Task.WhenAll(tasks);
                });

                await Task.Delay(200);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public async override Task OnDisappearingAsync()
        {
            IsVisible = false;
            await Task.CompletedTask;
        }

        async partial void OnCarouselPositionChanged(int oldValue, int newValue)
        {
            StateIndicatorVM.SelectState(newValue);
        }

        private async Task<bool> AskForConfirmation(string message)
        {
            return await Shell.Current.DisplayAlert("Potwierdzenie", message, "Tak", "Nie");
        }

        private async Task<bool> FinalizeUserAccountSetup()
        {
            try
            {
                var userVM = GetAccountSetupViewModel<AccountSetupUserContentViewModel>();
                var budgetVM = GetAccountSetupViewModel<AccountSetupBudgetContentViewModel>();
                var categoryVM = GetAccountSetupViewModel<AccountSetupCategoriesContentViewModel>();

                if (userVM == null || budgetVM == null || categoryVM == null)
                    return false;

                bool userResult = await UpdateUserAsync(userVM);
                if (!userResult)
                    return false;

                bool budgetResult = await CreateBudgetAsync(budgetVM);
                if (!budgetResult)
                    return false;

                var createdBudget = _budgetService.Budgets.FirstOrDefault();
                if (createdBudget == null)
                    return false;

                bool categoriesResult = await CreateTransactionCategoriesAsync(createdBudget, categoryVM);
                return categoriesResult;
            }
            catch (Exception ex)
            {
                //TODO: Handle exception (maybe log it)
                return false;
            }
        }

        private async Task<bool> UpdateUserAsync(AccountSetupUserContentViewModel userVM)
        {
            return await _userService.UpdateUserAsync(userVM.TemporaryUser);
        }

        private async Task<bool> CreateBudgetAsync(AccountSetupBudgetContentViewModel budgetVM)
        {
            return await _budgetService.CreateBudgetAsync(budgetVM.TemporaryBudget);
        }

        private async Task<bool> CreateTransactionCategoriesAsync(Budget createdBudget, AccountSetupCategoriesContentViewModel categoryVM)
        {
            return await _transactionCategoryService.CreateTransactionCategoriesAsync(
                createdBudget, categoryVM.TransactionCategories.ToList()
            );
        }

        private T? GetAccountSetupViewModel<T>() where T : AccountSetupCarouselItemViewModelBase
        {
            return AccountSetupCarouselVMs.OfType<T>().FirstOrDefault();
        }
    }
}
