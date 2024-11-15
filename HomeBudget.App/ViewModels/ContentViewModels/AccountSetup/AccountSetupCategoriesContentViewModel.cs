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

        public AccountSetupCategoriesContentViewModel() : this(App.Services.GetService<ITransactionCategoryService>()!)
        {
        }

        public AccountSetupCategoriesContentViewModel(ITransactionCategoryService transactionCategoryService)
        {
            _transactionCategoryService = transactionCategoryService;

            TemporaryTransactionCategory = new TransactionCategory() { BudgetId = string.Empty, Id = string.Empty };

            IconSelectVM = new IconSelectContentViewModel();
            IconSelectVM.SelectedIconChanged += TransactionCategoryIconSelectVM_SelectedIconChanged;

            TemporaryTransactionCategory.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
        }

        private void TransactionCategoryIconSelectVM_SelectedIconChanged(object? sender, EventArgs e)
        {
            if (IconSelectVM.SelectedIconItem != null)
            {
                TemporaryTransactionCategory.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
            }
        }

        public async override Task ResetView()
        {
            if (!IconSelectVM.IsPopulated)
            {
                IconSelectVM.ReloadData();
            }

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
