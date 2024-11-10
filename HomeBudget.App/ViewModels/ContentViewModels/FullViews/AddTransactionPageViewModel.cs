using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Extensions;
using HomeBudget.App.Models;
using HomeBudget.App.Models.Transactions;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;
using HomeBudget.App.Views;
using HomeBudget.App.Views.ContentViews.FullViews;
using Transaction = HomeBudget.App.Models.Transaction;

namespace HomeBudget.App.ViewModels.ContentViewModels.FullViews
{
    public partial class AddTransactionPageViewModel : BaseViewModel
    {
        [RelayCommand]
        public async Task Back()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync($"//{nameof(DashboardPageAndroidView)}");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task ProcessAction()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                if (ActionTypeSelectGroupVM.SelectedType is TransactionModifyActionType actionType)
                {
                    switch (actionType)
                    {
                        case TransactionModifyActionType.Create:
                            await CreateTransaction();
                            break;
                        case TransactionModifyActionType.Update:
                            await UpdateTransaction();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas wykonywania akcji.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [ObservableProperty]
        private SelectableButtonGroupViewModel _actionTypeSelectGroupVM;

        [ObservableProperty]
        private DropdownTransactionCategoryContentViewModel _categorySelectVM;

        [ObservableProperty]
        private Transaction _temporaryTransaction;

        [ObservableProperty]
        private DateTime _selectedDate;

        [ObservableProperty]
        private TimeSpan _selectedTime;

        private readonly IBudgetService _budgetService;
        private readonly IUserService _userService;
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly ITransactionService _transactionService;

        public AddTransactionPageViewModel(IBudgetService budgetService, IUserService userService, ITransactionCategoryService transactionCategoryService, ITransactionService transactionService)
        {
            Route = nameof(AddTransactionAndroidPageView);
            Title = "Zarządzanie transakcją";

            _budgetService = budgetService;
            _userService = userService;
            _transactionCategoryService = transactionCategoryService;
            _transactionService = transactionService;

            TemporaryTransaction = new Transaction()
            {
                Id = string.Empty,
                TransactionCategoryId = string.Empty,
                BudgetId = string.Empty,
                CreatorId = string.Empty
            };

            SelectedDate = TemporaryTransaction.Date;
            SelectedTime = TemporaryTransaction.Date.TimeOfDay;

            ActionTypeSelectGroupVM = new SelectableButtonGroupViewModel(Enum.GetValues(typeof(TransactionModifyActionType)).Cast<TransactionModifyActionType>().Select(a => new OptionItem(a.GetDescription(), a)).ToList());
            ActionTypeSelectGroupVM.SelectedChanged += ActionTypeSelectGroupVM_SelectedChanged;

            CategorySelectVM = new DropdownTransactionCategoryContentViewModel(true);
            CategorySelectVM.SelectedTransactionCategoryChanged += CategorySelectVM_SelectedTransactionCategoryChanged;
        }

        public async override Task OnAppearingAsync()
        {
            try
            {
                IsBusy = true;
                IsVisible = true;

                await Task.Delay(100);

                await ResetView();
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

        private async Task ResetView()
        {
            //await CategorySelectVM.ReloadData();

            TemporaryTransaction = new Transaction()
            {
                Id = string.Empty,
                TransactionCategoryId = _budgetService.CurrentBudget.TransactionCategories.First().Id,
                BudgetId = _budgetService.CurrentBudget.Id,
                CreatorId = _userService.CurrentUser.Id
            };

            SelectedDate = TemporaryTransaction.Date;
            SelectedTime = TemporaryTransaction.Date.TimeOfDay;
        }

        private async Task UpdateTransaction()
        {
            if (!TemporaryTransaction.IsNullOrEmpty())
            {
                var result = await _transactionService.UpdateTransactionAsync(_budgetService.CurrentBudget, TemporaryTransaction);

                if (result)
                {
                    await ResetView();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Ups... Coś poszło nie tak.", "Nie udało się zmodyfikować transakcji.", "OK");
                }
            }
        }

        private async Task CreateTransaction()
        {
            var validation = TemporaryTransaction.ToCreateRequest().IsRequestValid();
            if (validation.IsValid)
            {
                var result = await _transactionService.CreateTransactionAsync(_budgetService.CurrentBudget, TemporaryTransaction);

                if (result)
                {
                    await ResetView();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Ups... Coś poszło nie tak.", "Nie udało się dodać transakcji.", "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Błąd formularza", validation.ErrorMessage, "OK");
            }
        }

        partial void OnSelectedDateChanged(DateTime oldValue, DateTime newValue)
        {
            TemporaryTransaction.Date = new DateTime(newValue.Year, newValue.Month, newValue.Day, newValue.Hour, newValue.Minute, newValue.Second);
        }

        partial void OnSelectedTimeChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            TemporaryTransaction.Date = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, newValue.Hours, newValue.Minutes, newValue.Seconds);
        }

        private void CategorySelectVM_SelectedTransactionCategoryChanged(object? sender, EventArgs e)
        {
            if (sender is TransactionCategory transactionCategory)
            {
                TemporaryTransaction.TransactionCategoryId = transactionCategory.Id;
                TemporaryTransaction.Name = transactionCategory.Name;
            }
        }

        private void ActionTypeSelectGroupVM_SelectedChanged(object? sender, EventArgs e)
        {
            if (sender is SelectableButtonGroupViewModel selectableVM)
            {
                if (selectableVM.SelectedType is TransactionModifyActionType actionType)
                {
                    switch (actionType)
                    {
                        case TransactionModifyActionType.Create:

                            TemporaryTransaction = new Transaction()
                            {
                                Id = string.Empty,
                                TransactionCategoryId = string.Empty,
                                BudgetId = _budgetService.CurrentBudget.Id,
                                CreatorId = _userService.CurrentUser.Id
                            };

                            break;
                        case TransactionModifyActionType.Update:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
