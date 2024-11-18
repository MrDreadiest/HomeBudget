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

        private readonly IBudgetService _budgetService;

        public AccountSetupBudgetContentViewModel(IconSelectContentViewModel iconSelectVM)
        {
            TemporaryBudget = new Budget() { Id = string.Empty, OwnerId = string.Empty };
            IconSelectVM = iconSelectVM;
        }

        public override void OnAppearing()
        {
            IsVisible = true;
            IconSelectVM.SelectedIconChanged += BudgetIconSelectVM_SelectedIconChanged;
            IconSelectVM.ResetView();
        }

        public override void OnDisappearing()
        {
            IsVisible = false;
            if (IconSelectVM.IsVisible)
            {
                IconSelectVM.ToggleView();
            }
            IconSelectVM.SelectedIconChanged -= BudgetIconSelectVM_SelectedIconChanged;
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
