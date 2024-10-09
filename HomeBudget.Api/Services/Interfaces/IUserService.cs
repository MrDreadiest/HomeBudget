using HomeBudget.Common.EntityDTOs.User;

namespace HomeBudget.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserGetResponseModel?> GetUserByIdAsync(string userId);
        Task<UserUpdateResponseModel?> UpdateUserAsync(string userId, UserUpdateRequestModel requestModel);
        Task<bool> DeleteUserAsync(string userId);
    }
}
