using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Extensions;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.AccountSetup
{
    public partial class AccountSetupCategoriesContentViewModel : AccountSetupCarouselItemViewModelBase
    {
        [RelayCommand]
        public void RemoveTransactionCategory(TransactionCategory transactionCategory)
        {
            TransactionCategories.Remove(transactionCategory);
        }

        [RelayCommand]
        public void AddTransactionCategory()
        {
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
                IconUnicode = IconSelectVM.SelectedIconItem.Unicode
            };
        }

        [ObservableProperty]
        private TransactionCategory _temporaryTransactionCategory;

        [ObservableProperty]
        private ObservableCollection<TransactionCategory> _transactionCategories = new();

        public IconSelectContentViewModel IconSelectVM { get; set; }

        private readonly ITransactionCategoryService _transactionCategoryService;

        public AccountSetupCategoriesContentViewModel(IconSelectContentViewModel iconSelectVM)
        {
            TemporaryTransactionCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty };
            IconSelectVM = iconSelectVM;
        }

        private void TransactionCategoryIconSelectVM_SelectedIconChanged(object? sender, EventArgs e)
        {
            if (IconSelectVM.SelectedIconItem != null)
            {
                TemporaryTransactionCategory.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
            }
        }

        public override void OnAppearing()
        {
            IsVisible = true;
            IconSelectVM.SelectedIconChanged += TransactionCategoryIconSelectVM_SelectedIconChanged;
            IconSelectVM.ResetView();
        }

        public override void OnDisappearing()
        {
            IsVisible = false;
            if (IconSelectVM.IsVisible)
            {
                IconSelectVM.ToggleView();
            }
            IconSelectVM.SelectedIconChanged -= TransactionCategoryIconSelectVM_SelectedIconChanged;
        }

        public async override Task ResetView()
        {
            TransactionCategories.Clear();

            TemporaryTransactionCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty };
            TemporaryTransactionCategory.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;

            await Task.CompletedTask;
        }

        public override bool CanGoNext()
        {
            int positive = TransactionCategories.Where(e => e.ToCreateRequest().IsRequestValid()).Count();
            return positive == TransactionCategories.Count();
        }
    }
}
