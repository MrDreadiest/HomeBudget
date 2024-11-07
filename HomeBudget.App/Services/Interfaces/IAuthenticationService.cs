using HomeBudget.Common.EntityDTOs.Account;

namespace HomeBudget.App.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public event EventHandler LoggedIn;
        public event EventHandler LoggedOut;

        Task<bool> LoginAsync(LoginRequestModel request);
        Task<bool> LogoutAsync();
    }
}
