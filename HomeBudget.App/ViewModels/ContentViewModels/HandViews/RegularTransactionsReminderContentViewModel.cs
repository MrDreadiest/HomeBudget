using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.ViewModels.Widgets;
using HomeBudget.App.Views.ContentViews.FullViews;

namespace HomeBudget.App.ViewModels.ContentViewModels.HandViews
{
    public partial class RegularTransactionsReminderContentViewModel : WidgetContentViewModelBase
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

        internal async Task Reload()
        {
            throw new NotImplementedException();
        }

        public override void LoadConfiguration()
        {
            throw new NotImplementedException();
        }

        public override void SaveConfiguration()
        {
            throw new NotImplementedException();
        }

        public RegularTransactionsReminderContentViewModel()
        {
            // TODO: Przeniesienie do zasobów
            Title = "Cykliczne transakcje";
            FullViewRoute = nameof(ManageRegularTransactionsAndroidPageView);

        }
    }
}
