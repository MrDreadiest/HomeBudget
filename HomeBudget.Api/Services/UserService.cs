using HomeBudget.Api.Entities;
using HomeBudget.Api.Extensions;
using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Api.UnitOfWork.Interfaces;
using HomeBudget.Common.EntityDTOs.User;
using System.Text.RegularExpressions;

namespace HomeBudget.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserGetResponseModel?> GetUserByIdAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);

            return user == null ? null : user.ToGetResponse();
        }

        public async Task<UserUpdateResponseModel?> UpdateUserAsync(string userId, UserUpdateRequestModel requestModel)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);

            if (user != null && requestModel != null)
            {
                user.FirstName = requestModel.FirstName;
                user.LastName = requestModel.LastName;
                user.IsAccountConfirmed = requestModel.IsAccountConfirmed;
                user.IsAccountSetup = requestModel.IsAccountSetup;
                user.Email = requestModel.Email;
                user.PhoneNumber = requestModel.PhoneNumber;

                user.IsAccountSetup = IsUserSetup(user);

                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveChangesAsync();

                return user.ToUpdateResponse();
            }
            return null;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user != null)
            {
                _unitOfWork.Users.Delete(user);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private bool IsUserSetup(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName))
                return false;

            if (string.IsNullOrWhiteSpace(user.LastName))
                return false;

            if (!IsValidPhoneNumber(user.PhoneNumber))
                return false;

            return true;
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var phonePattern = @"^\d{7,15}$";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }
    }
}
