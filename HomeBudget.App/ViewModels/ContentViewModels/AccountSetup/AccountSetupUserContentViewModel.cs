using CommunityToolkit.Mvvm.ComponentModel;
using HomeBudget.App.Extensions;
using HomeBudget.App.Models;
using HomeBudget.App.Services.Interfaces;

namespace HomeBudget.App.ViewModels.ContentViewModels.AccountSetup
{
    public partial class AccountSetupUserContentViewModel : AccountSetupCarouselItemViewModelBase
    {
        [ObservableProperty]
        private User _temporaryUser;

        [ObservableProperty]
        private Address _temporaryAddress;

        private readonly IUserService _userService;

        public AccountSetupUserContentViewModel(IUserService userService)
        {
            _userService = userService;

            TemporaryUser = new User() { Id = string.Empty };
            TemporaryAddress = new Address() { Id = string.Empty, UserId = string.Empty };
        }

        public AccountSetupUserContentViewModel() : this(App.Services.GetService<IUserService>()!)
        {
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
