
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.UniversalControls
{
    public partial class DropdownTransactionCategoryContentViewModel : BaseViewModel
    {
        public event EventHandler? SelectedTransactionCategoryChanged;

        [RelayCommand]
        public async Task ToggleView()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                ToggleVisibility();
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
        private ObservableCollection<TransactionCategory> _transactionCategories;

        [ObservableProperty]
        private ObservableCollection<TransactionCategory> _filteredTransactionCategories;

        [ObservableProperty]
        private TransactionCategory _selectedCategory;

        [ObservableProperty]
        private ObservableCollection<TransactionCategory> _selectedCategories;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private SelectionMode _selectionMode;

        public bool IsPopulated => TransactionCategories.Count > 0;

        public DropdownTransactionCategoryContentViewModel(SelectionMode selectionMode)
        {
            SelectionMode = selectionMode;
            IsVisible = false;
            TransactionCategories = new ObservableCollection<TransactionCategory>();
            FilteredTransactionCategories = new ObservableCollection<TransactionCategory>();
            SelectedCategory = new TransactionCategory() { Id = string.Empty, BudgetId = string.Empty };
            SelectedCategories = new ObservableCollection<TransactionCategory>();
        }

        public async Task PopulateCollection(List<TransactionCategory> transactionCategories)
        {
            await Task.Run(() =>
            {
                TransactionCategories.Clear();
                foreach (var transactionCategory in transactionCategories)
                {
                    TransactionCategories.Add(transactionCategory);
                }

                FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(TransactionCategories);
                SelectedCategory = TransactionCategories.First();
            });
        }

        public override Task OnAppearingAsync() => Task.CompletedTask;
        public override Task OnDisappearingAsync() => Task.CompletedTask;

        partial void OnSearchTextChanging(string? oldValue, string newValue)
        {
            FilterTransactionCategories(newValue);
        }

        partial void OnSelectedCategoryChanged(TransactionCategory? oldValue, TransactionCategory newValue)
        {
            SelectedTransactionCategoryChanged?.Invoke(newValue, EventArgs.Empty);
        }

        partial void OnSelectedCategoriesChanged(ObservableCollection<TransactionCategory>? oldValue, ObservableCollection<TransactionCategory> newValue)
        {

        }

        private void ToggleVisibility() => IsVisible = !IsVisible;

        private void FilterTransactionCategories(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(TransactionCategories);
            }
            else
            {
                var filteredItems = TransactionCategories
                .Where(tc => tc.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

                FilteredTransactionCategories = new ObservableCollection<TransactionCategory>(filteredItems);
            }
        }
    }
}
