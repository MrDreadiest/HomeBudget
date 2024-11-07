using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels
{
    public partial class TransactionListContentViewModel : BaseContentViewModel
    {
        public event EventHandler? RefreshViewCall;

        [RelayCommand]
        public async Task RefreshView()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                RefreshViewCall?.Invoke(this, EventArgs.Empty);
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

                if (!await Remove(transaction))
                {
                    await Shell.Current.DisplayAlert("Błąd", "Nie udało się usunąć kategorii.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas usuwania kategorii.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        [RelayCommand]
        public async Task EditTransaction(TransactionGroupItem transaction)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", "Wystąpił błąd podczas usuwania kategorii.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
            await Task.CompletedTask;
        }

        [ObservableProperty]
        private ObservableCollection<TransactionGroup> _groupedTransactions;

        [ObservableProperty]
        private ObservableCollection<TransactionGroup> _allTransactions;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private SwipeView _currentOpenSwipeView;

        [ObservableProperty]
        private string _currentGroupTitle;

        private readonly IBudgetService _budgetService;
        private readonly ITransactionService _transactionService;

        public TransactionListContentViewModel(IBudgetService budgetService, ITransactionService transactionService)
        {
            _budgetService = budgetService;
            _transactionService = transactionService;

            GroupedTransactions = new ObservableCollection<TransactionGroup>();
            AllTransactions = new ObservableCollection<TransactionGroup>();
            CurrentGroupTitle = DateTime.Now.ToString("dd MMMM yyyy");
        }

        internal void ReloadTransactions()
        {
            AllTransactions.Clear();
            GroupedTransactions.Clear();

            Budget budget = _budgetService.CurrentBudget;

            var grouped = budget.Transactions
                .OrderByDescending(t => t.Date)
                .GroupBy(t => t.Date.Date)
                .Select(g => new TransactionGroup(g.Key, new ObservableCollection<TransactionGroupItem>(g.Select(t => new TransactionGroupItem(t, budget.TransactionCategories.Find(c => c.Id.Equals(t.TransactionCategoryId))!)))))
                .ToList();

            foreach (var group in grouped)
            {
                AllTransactions.Add(group);
                GroupedTransactions.Add(group);
            }
        }

        internal void SearchTransactions()
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var filtered = GroupedTransactions
                    .Select(group => new TransactionGroup(
                        group.Date,
                        new ObservableCollection<TransactionGroupItem>(
                            group.Where(t => t.Transaction.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                             t.Transaction.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                        )))
                    .Where(group => group.Any())
                    .ToList();

                GroupedTransactions = new ObservableCollection<TransactionGroup>(filtered);
            }
        }

        private async Task<bool> Remove(TransactionGroupItem transaction)
        {
            bool result = await _transactionService.DeleteTransactionAsync(_budgetService.CurrentBudget, transaction.Transaction);
            if (result)
            {
                RemoveLocally(transaction);
            }
            return result;
        }

        private void RemoveLocally(TransactionGroupItem transaction)
        {
            foreach (var group in GroupedTransactions)
            {
                if (group.Contains(transaction))
                {
                    group.Remove(transaction);
                    if (group.Count == 0)
                    {
                        GroupedTransactions.Remove(group);
                    }
                    break;
                }
            }
        }

        internal void ToggleSwipeView(SwipeView? swipeView)
        {
            if (CurrentOpenSwipeView != null && CurrentOpenSwipeView != swipeView)
            {
                CurrentOpenSwipeView.Close();
            }

            CurrentOpenSwipeView = swipeView;
        }

        internal void FilterTransactions(List<TransactionGroup> transactionGroups)
        {
            GroupedTransactions = new ObservableCollection<TransactionGroup>(transactionGroups);
        }

        partial void OnGroupedTransactionsChanged(ObservableCollection<TransactionGroup> value)
        {
            CurrentGroupTitle = GroupedTransactions.FirstOrDefault()?.Date.ToString("dd MMMM yyyy") ?? DateTime.Now.ToString("dd MMMM yyyy");
        }
    }


    public class TransactionGroup : ObservableCollection<TransactionGroupItem>
    {
        public DateTime Date { get; }

        public TransactionGroup(DateTime date, ObservableCollection<TransactionGroupItem> transactions) : base(transactions)
        {
            Date = date;
        }
    }

    public class TransactionGroupItem
    {
        public Transaction Transaction { get; }

        public TransactionCategory TransactionCategory { get; }

        public TransactionGroupItem(Transaction transaction, TransactionCategory transactionCategory)
        {
            Transaction = transaction;
            TransactionCategory = transactionCategory;
        }
    }
}
