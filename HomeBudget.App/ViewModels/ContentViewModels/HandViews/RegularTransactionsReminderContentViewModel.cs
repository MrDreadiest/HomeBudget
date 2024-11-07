using CommunityToolkit.Mvvm.Input;
using HomeBudget.App.Views.ContentViews.FullViews;
using HomeBudget.App.Views.ContentViews.HandViews;

namespace HomeBudget.App.ViewModels.ContentViewModels.HandViews
{
    public partial class RegularTransactionsReminderContentViewModel : HandViewBaseModel, IHandViewBaseModel
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

        public RegularTransactionsReminderContentViewModel()
        {
            // TODO: Przeniesienie do zasobów
            Title = "Cykliczne transakcje";
            Route = nameof(RegularTransactionsReminderContentView);
            FullViewRoute = nameof(ManageRegularTransactionsAndroidPageView);

        }
    }
}
