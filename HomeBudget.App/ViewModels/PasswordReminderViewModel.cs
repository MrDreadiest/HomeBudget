using HomeBudget.App.Services;

namespace HomeBudget.App.ViewModels
{
    public partial class PasswordReminderViewModel : BaseViewModel
    {

        public PasswordReminderViewModel()
        {

        }

        public async override Task OnAppearingAsync()
        {
            IsVisible = true;
            await Task.CompletedTask;
        }

        public async override Task OnDisappearingAsync()
        {
            IsVisible = false;
            await Task.CompletedTask;
        }
    }
}
