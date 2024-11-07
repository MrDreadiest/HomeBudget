
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Extensions;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls;
using HomeBudget.App.Views;
using HomeBudget.App.Views.ContentViews.FullViews;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.FullViews
{
    public partial class ManageTransactionsSplitPageViewModel : BaseViewModel
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
        public async Task Add()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var result = await _transactionService.CreateTransactionsAsync(_budgetService.CurrentBudget, Transactions.Select(i => i.Transaction).ToList());

                if (result)
                {
                    await ResetView();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Ups... Coś poszło nie tak.", "Nie udało się dodać transakcji.", "OK");
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
        public async Task RemoveTransaction(TransactionGroupItem transaction)
        {
            if (IsBusy)
                return;

            bool isConfirmed = await Shell.Current.DisplayAlert(
                "Potwierdzenie usunięcia",
                "Czy na pewno chcesz usunąć tę transakcję?",
                "Tak",
                "Nie"
            );

            if (!isConfirmed)
                return;

            try
            {
                IsBusy = true;
                AmountToSplit += transaction.Transaction.TotalAmount;
                Transactions.Remove(transaction);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas usuwania kategorii.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task Split()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

                var result = CanSplit();

                if (result.Flag)
                {
                    Transactions.Add(new TransactionGroupItem(TemporaryTransaction, CategorySelectVM.SelectedCategory));

                    AmountToSplit -= TemporaryTransaction.TotalAmount;

                    TemporaryTransaction = new Transaction()
                    {
                        Id = string.Empty,
                        TransactionCategoryId = _budgetService.CurrentBudget.TransactionCategories.First().Id,
                        BudgetId = _budgetService.CurrentBudget.Id,
                        CreatorId = _userService.CurrentUser.Id
                    };
                }
                else
                {
                    await Shell.Current.DisplayAlert("Błąd formularza", result.Message, "OK");
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
        public async Task PushRest()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                TemporaryTransaction.TotalAmount = AmountToSplit;
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


        private (bool Flag, string Message) CanSplit()
        {
            if (TemporaryTransaction.TotalAmount > AmountToSplit)
                return (false, "Kwota do wydzielenia przewyższa pozostałą.");

            var validation = TemporaryTransaction.ToCreateRequest().IsRequestValid();
            if (!validation.IsValid)
                return (validation.IsValid, validation.ErrorMessage);

            return (true, string.Empty);
        }

        [ObservableProperty]
        private DateTime _selectedDate;

        [ObservableProperty]
        private TimeSpan _selectedTime;

        [ObservableProperty]
        private Transaction _temporaryTransaction;

        [ObservableProperty]
        private DropdownTransactionCategoryContentViewModel _categorySelectVM;

        [ObservableProperty]
        private CalculatorPopupContentViewModel _calculatorPopupContentVM;

        [ObservableProperty]
        private decimal _amountToSplit;

        [ObservableProperty]
        private ObservableCollection<TransactionGroupItem> _transactions;

        [ObservableProperty]
        private SwipeView _currentOpenSwipeView;

        private readonly IBudgetService _budgetService;
        private readonly IUserService _userService;
        private readonly ITransactionCategoryService _transactionCategoryService;
        private readonly ITransactionService _transactionService;

        public ManageTransactionsSplitPageViewModel(IBudgetService budgetService, IUserService userService, ITransactionCategoryService transactionCategoryService, ITransactionService transactionService)
        {
            Route = nameof(ManageTransactionsSplitAndroidPageView);
            Title = "Dodaj z podziałem";

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

            Transactions = new ObservableCollection<TransactionGroupItem>();

            SelectedDate = TemporaryTransaction.Date;
            SelectedTime = TemporaryTransaction.Date.TimeOfDay;

            CalculatorPopupContentVM = new CalculatorPopupContentViewModel();
            CalculatorPopupContentVM.CalculationCompleted += CalculatorPopupContentVM_CalculationCompleted;

            CategorySelectVM = new DropdownTransactionCategoryContentViewModel(SelectionMode.Single);
            CategorySelectVM.SelectedTransactionCategoryChanged += CategorySelectVM_SelectedTransactionCategoryChanged;

        }

        public async override Task OnAppearingAsync()
        {
            try
            {
                IsBusy = true;
                IsVisible = true;

                await ReloadData();
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
            try
            {
                IsBusy = true;
                IsVisible = false;
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

        internal void ToggleSwipeView(SwipeView? swipeView)
        {
            if (CurrentOpenSwipeView != null && CurrentOpenSwipeView != swipeView)
            {
                CurrentOpenSwipeView.Close();
            }

            CurrentOpenSwipeView = swipeView;
        }

        partial void OnSelectedDateChanged(DateTime oldValue, DateTime newValue)
        {
            TemporaryTransaction.Date = new DateTime(newValue.Year, newValue.Month, newValue.Day, newValue.Hour, newValue.Minute, newValue.Second);
        }

        partial void OnSelectedTimeChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            TemporaryTransaction.Date = new DateTime(SelectedDate.Year, SelectedDate.Month, SelectedDate.Day, newValue.Hours, newValue.Minutes, newValue.Seconds);
        }

        private async Task ReloadData()
        {
            await _transactionCategoryService.GetAllTransactionCategoriesAsync(_budgetService.CurrentBudget);
        }

        private async Task ResetView()
        {
            TemporaryTransaction = new Transaction()
            {
                Id = string.Empty,
                TransactionCategoryId = _budgetService.CurrentBudget.TransactionCategories.First().Id,
                BudgetId = _budgetService.CurrentBudget.Id,
                CreatorId = _userService.CurrentUser.Id
            };

            AmountToSplit = 0;

            SelectedDate = TemporaryTransaction.Date;
            SelectedTime = TemporaryTransaction.Date.TimeOfDay;

            Transactions.Clear();

            await CategorySelectVM.PopulateCollection(_budgetService.CurrentBudget.TransactionCategories);
        }

        private void CategorySelectVM_SelectedTransactionCategoryChanged(object? sender, EventArgs e)
        {
            if (sender is TransactionCategory transactionCategory)
            {
                TemporaryTransaction.TransactionCategoryId = transactionCategory.Id;
                TemporaryTransaction.Name = transactionCategory.Name;
            }
        }

        private void CalculatorPopupContentVM_CalculationCompleted(object? sender, decimal value)
        {
            TemporaryTransaction.TotalAmount = value;
        }
    }
}
