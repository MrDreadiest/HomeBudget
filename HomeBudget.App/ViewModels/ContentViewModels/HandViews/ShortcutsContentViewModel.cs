using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Resources.Icons;
using HomeBudget.App.ViewModels.ContentViewModels.Shortcuts;
using HomeBudget.App.ViewModels.Widgets;
using HomeBudget.App.Views.ContentViews.FullViews;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.HandViews
{
    public partial class ShortcutsContentViewModel : WidgetContentViewModelBase
    {
        [RelayCommand]
        public async Task GoToFullView()
        {
            if (IsBusy)
                return;
            try
            {
                await Shell.Current.GoToAsync($"//{FullViewRoute}");
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

        public override void LoadConfiguration()
        {
            throw new NotImplementedException();
        }

        public override void SaveConfiguration()
        {
            throw new NotImplementedException();
        }

        [ObservableProperty]
        private ObservableCollection<ShortcutBaseContentViewModel> _shortcuts;

        public ShortcutsContentViewModel()
        {
            // TODO: Przeniesienie do zasobów
            Title = "Skróty";
            FullViewRoute = nameof(ManageShortcutsAndroidPageView);

            Shortcuts = new ObservableCollection<ShortcutBaseContentViewModel>()
            {
                new ShortcutButtonContentViewModel(ShortcutType.SplitTransaction.GetDescription(), nameof(ManageTransactionsSplitAndroidPageView)) { IconUnicode = Icons.TransactionSplit },
                new ShortcutButtonContentViewModel(ShortcutType.AddTransaction.GetDescription(), nameof(AddTransactionAndroidPageView)) { IconUnicode = Icons.TransactionAdd },
                new ShortcutButtonContentViewModel(ShortcutType.ManageCategory.GetDescription(), nameof(ManageCategoriesAndroidPageView)) { IconUnicode = Icons.Categories },
            };
        }
    }
}
