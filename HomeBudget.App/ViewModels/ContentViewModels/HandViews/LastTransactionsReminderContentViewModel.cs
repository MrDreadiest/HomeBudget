using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;
using HomeBudget.App.ViewModels.ContentViewModels.UniversalControls.CollapseHelper;
using HomeBudget.App.ViewModels.Widgets;
using HomeBudget.App.Views;
using System.Collections.ObjectModel;

namespace HomeBudget.App.ViewModels.ContentViewModels.HandViews
{
    public partial class LastTransactionsReminderContentViewModel : WidgetContentViewModelBase, ICollapseContentViewModel
    {
        private const int LastTransactionReminderCount = 6;

        [RelayCommand]
        public void ToggleIsCollapsed()
        {
            IsCollapsed = !IsCollapsed;
        }

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
        }

        [ObservableProperty]
        private ObservableCollection<TransactionGroupItem> _transactions;

        [ObservableProperty]
        private int _reminderListCount = LastTransactionReminderCount;

        private readonly IAppSettingsService _appSettingsService;

        public LastTransactionsReminderContentViewModel(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;

            // TODO: Przeniesienie do zasobów
            Title = "Ostatnie transakcje";
            FullViewRoute = nameof(TransactionsPageAndroidView);

            Transactions = new ObservableCollection<TransactionGroupItem>();
        }

        internal async Task Reload(List<Transaction> transactions, List<TransactionCategory> transactionCategories)
        {
            Transactions.Clear();
            await Task.Run(() =>
            {
                var sortedRange = transactions.OrderByDescending(t => t.Date).ToList().GetRange(0, ReminderListCount);
                foreach (var transaction in sortedRange)
                {
                    Transactions.Add(new TransactionGroupItem(transaction, transactionCategories.Find(c => c.Id.Equals(transaction.TransactionCategoryId))!));
                }
            });
        }

        public override void LoadConfiguration()
        {
            throw new NotImplementedException();
        }

        public override void SaveConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
