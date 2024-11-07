using HomeBudget.Common.EntityDTOs.Account;

namespace HomeBudget.App.Services.Interfaces
{
    public interface IRegisterService
    {
        Task<bool> RegisterAsync(RegisterRequestModel requestModel);
    }
}
