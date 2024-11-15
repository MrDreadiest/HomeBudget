using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.Extensions;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;

namespace HomeBudget.App.ViewModels.ContentViewModels.AccountSetup
{
    public partial class AccountSetupBudgetContentViewModel : AccountSetupCarouselItemViewModelBase
    {
        [ObservableProperty]
        private Budget _temporaryBudget;

        public IconSelectContentViewModel IconSelectVM { get; set; }

        private readonly IBudgetService _budgetService;

        public AccountSetupBudgetContentViewModel() : this(App.Services.GetService<IBudgetService>()!)
        {
        }

        public AccountSetupBudgetContentViewModel(IBudgetService budgetService)
        {
            _budgetService = budgetService;

            TemporaryBudget = new Budget() { Id = string.Empty, OwnerId = string.Empty };

            IconSelectVM = new IconSelectContentViewModel();
            IconSelectVM.SelectedIconChanged += BudgetIconSelectVM_SelectedIconChanged;

            TemporaryBudget.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;

        }

        private void BudgetIconSelectVM_SelectedIconChanged(object? sender, EventArgs e)
        {
            if (IconSelectVM.SelectedIconItem != null)
            {
                TemporaryBudget.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;
            }
        }

        public async override Task ResetView()
        {
            if (!IconSelectVM.IsPopulated)
            {
                IconSelectVM.ReloadData();
            }

            TemporaryBudget = new Budget() { Id = string.Empty, OwnerId = string.Empty };
            TemporaryBudget.IconUnicode = IconSelectVM.SelectedIconItem.Unicode;

            await Task.CompletedTask;
        }

        public override bool CanGoNext()
        {
            return TemporaryBudget.ToCreateRequest().IsCreateRequestValid();
        }
    }
}
