using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Extensions;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels;
using HomeBudget.App.Views;
using System.Collections.ObjectModel;


namespace HomeBudget.App.ViewModels
{
    public partial class UserAccountSetupPageViewModel : BaseViewModel
    {
        [RelayCommand]
        public async Task Back()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                switch (_currentView)
                {
                    case ContentView.Account:
                        var result = await _authenticationService.LogoutAsync();
                        if (result)
                        {
                            await Shell.Current.GoToAsync($"//{nameof(MainPageAndroidView)}");
                        }
                        break;
                    case ContentView.Budget:
                        ChangeView(ContentView.Account);
                        break;
                    case ContentView.Categories:
                        ChangeView(ContentView.Budget);
                        break;
                    default:
                        break;
                }
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

        [RelayCommand]
        public async Task Next()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                if (CheckCanGoNext())
                {
                    switch (_currentView)
                    {
                        case ContentView.Account:
                            ChangeView(ContentView.Budget);
                            break;
                        case ContentView.Budget:
                            ChangeView(ContentView.Categories);
                            break;
                        case ContentView.Categories:
                            bool result = await FinalizeUserAccountSetup();
                            if (result)
                            {
                                await Shell.Current.GoToAsync($"//{nameof(DashboardPageAndroidView)}");
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    // TODO: Przenieść do zasobów
                    await Shell.Current.DisplayAlert("Błąd formularza", "Formularz nie został wypełniony poprawnie.", "Ok");
                }
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

        [RelayCommand]
        public async Task ToggleBudgetIconSelect()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                BudgetIconSelectVM.ToggleVisibility();
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

        [RelayCommand]
        public async Task ToggleTransactionCategoryIconSelect()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                TransactionCategoryIconSelectVM.ToggleVisibility();
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

        [RelayCommand]
        public async Task AddTransactionCategory()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                TransactionCategories.Add(
                    new TransactionCategory() 
                    { 
                        Id = string.Empty,
                        BudgetId = string.Empty,
                        Name = TemporaryTransactionCategory.Name,
                        IconUnicode = TemporaryTransactionCategory.IconUnicode
                    }
                );

                TemporaryTransactionCategory = new TransactionCategory() 
                { 
                    BudgetId = string.Empty, 
                    Id = string.Empty, 
                    IconUnicode = TransactionCategoryIconSelectVM.SelectedIconItem.Unicode 
                };
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

        [RelayCommand]
        public async Task RemoveTransactionCategory(TransactionCategory transactionCategory)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                TransactionCategories.Remove(transactionCategory);
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
        private bool isAccountSetupUserContentViewVisible;

        [ObservableProperty]
        private bool isAccountSetupBudgetContentViewVisible;

        [ObservableProperty]
        private bool isAccountSetupCategoriesContentViewVisible;

        [ObservableProperty]
        private IconSelectContentViewModel budgetIconSelectVM;

        [ObservableProperty]
        private IconSelectContentViewModel transactionCategoryIconSelectVM;

        [ObservableProperty]
        private bool canGoNext;

        [ObservableProperty]
        private User temporaryUser;

        [ObservableProperty]
        private Address temporaryAddress;

        [ObservableProperty]
        private Budget temporaryBudget;

        [ObservableProperty]
        private TransactionCategory temporaryTransactionCategory;

        [ObservableProperty]
        private ObservableCollection<TransactionCategory> transactionCategories;

        private ContentView _currentView;
        
        private readonly IUserService _userService;
        private readonly IBudgetService _budgetService;
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly IAuthenticationService _authenticationService;

        public UserAccountSetupPageViewModel(IUserService userService , IBudgetService budgetService, ITransactionCategoryService transactionCategoryService,IAuthenticationService authenticationService)
        {
            _userService = userService;
            _budgetService = budgetService;
            _transactionCategoryService = transactionCategoryService;

            _authenticationService = authenticationService;

            TemporaryUser = new User() { Id = string.Empty };
            TemporaryAddress = new Address() { Id = string.Empty, UserId = string.Empty };

            TemporaryBudget = new Budget() { Id = string.Empty, OwnerId = string.Empty};
            TemporaryTransactionCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty};

            TransactionCategories = new ObservableCollection<TransactionCategory>();

            BudgetIconSelectVM = new IconSelectContentViewModel();
            BudgetIconSelectVM.SelectedIconChanged += BudgetIconSelectVM_SelectedIconChanged;

            TransactionCategoryIconSelectVM = new IconSelectContentViewModel();
            TransactionCategoryIconSelectVM.SelectedIconChanged += TransactionCategoryIconSelectVM_SelectedIconChanged; ;

            TemporaryBudget.IconUnicode = BudgetIconSelectVM.SelectedIconItem.Unicode;
            TemporaryTransactionCategory.IconUnicode = TransactionCategoryIconSelectVM.SelectedIconItem.Unicode;
        }

        public async override Task OnAppearingAsync()
        {
            IsVisible = true;
            ResetView();
            bool result = await _budgetService.GetAllBudgetsAsync();
            if (result) 
            {
                var budgets = _budgetService.Budgets;

            }

            await Task.CompletedTask;
        }

        public async override Task OnDisappearingAsync()
        {
            IsVisible = false;
            await Task.CompletedTask;
        }

        private void ResetView()
        {
            _currentView = ContentView.Account;

            IsAccountSetupUserContentViewVisible = true;
            IsAccountSetupBudgetContentViewVisible = false;
            IsAccountSetupCategoriesContentViewVisible = false;

            CanGoNext = CheckCanGoNext();
        }

        private void ChangeView(ContentView view)
        {
            _currentView = view;
            
            IsAccountSetupUserContentViewVisible = false;
            IsAccountSetupBudgetContentViewVisible = false;
            IsAccountSetupCategoriesContentViewVisible = false;

            switch (_currentView)
            {
                case ContentView.Account:
                    IsAccountSetupUserContentViewVisible = true;
                    break;
                case ContentView.Budget:
                    IsAccountSetupBudgetContentViewVisible = true;
                    break;
                case ContentView.Categories:
                    IsAccountSetupCategoriesContentViewVisible = true;
                    break;
                default:
                    break;
            }
        }

        private bool CheckCanGoNext()
        {
            switch (_currentView)
            {
                case ContentView.Account:
                    return TemporaryUser.ToUpdateRequest().IsUpdateRequestValid();
                case ContentView.Budget:
                    return TemporaryBudget.ToCreateRequest().IsCreateRequestValid();
                case ContentView.Categories:
                    int positive = TransactionCategories.Where(e => e.ToCreateRequest().IsRequestValid()).Count();
                    return positive == TransactionCategories.Count();
                default:
                    break;
            }
            return false;
        }

        private async Task<bool> FinalizeUserAccountSetup()
        {
            bool userResult = await _userService.UpdateUserAsync(TemporaryUser);

            bool budgetResult = await _budgetService.CreateBudgetAsync(TemporaryBudget);

            bool transactionCategoriesResult = false;
            if (budgetResult)
            {
                transactionCategoriesResult = await _transactionCategoryService.CreateTransactionCategoriesAsync(_budgetService.Budgets.First(), TransactionCategories.ToList());
            }

            return userResult && budgetResult && transactionCategoriesResult;
        }


        private void TransactionCategoryIconSelectVM_SelectedIconChanged(object? sender, EventArgs e)
        {
            TemporaryTransactionCategory.IconUnicode = TransactionCategoryIconSelectVM.SelectedIconItem.Unicode;
        }

        private void BudgetIconSelectVM_SelectedIconChanged(object? sender, EventArgs e)
        {
            if (BudgetIconSelectVM.SelectedIconItem != null)
            {
                TemporaryBudget.IconUnicode = BudgetIconSelectVM.SelectedIconItem.Unicode;
            }
        }

        public enum ContentView
        {
            Account,
            Budget,
            Categories
        }
    }
}
