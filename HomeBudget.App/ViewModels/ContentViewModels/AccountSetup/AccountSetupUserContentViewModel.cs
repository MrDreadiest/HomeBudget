using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.Extensions;
using HomeBudget.App.Models;

namespace HomeBudget.App.ViewModels.ContentViewModels.AccountSetup
{
    public partial class AccountSetupUserContentViewModel : AccountSetupCarouselItemViewModelBase
    {
        [ObservableProperty]
        private User _temporaryUser;

        [ObservableProperty]
        private Address _temporaryAddress;

        public AccountSetupUserContentViewModel()
        {
            TemporaryUser = new User() { Id = string.Empty };
            TemporaryAddress = new Address() { Id = string.Empty, UserId = string.Empty };
        }

        public override void OnAppearing()
        {
            IsVisible = true;
        }

        public override void OnDisappearing()
        {
            IsVisible = false;
        }

        public async override Task ResetView()
        {
            TemporaryUser = new User() { Id = string.Empty };
            TemporaryAddress = new Address() { Id = string.Empty, UserId = string.Empty };
            await Task.CompletedTask;
        }

        public override bool CanGoNext()
        {
            return TemporaryUser.ToUpdateRequest().IsUpdateRequestValid();
        }
    }
}