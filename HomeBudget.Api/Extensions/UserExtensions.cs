using HomeBudget.Api.Entities;
using HomeBudget.Common.EntityDTOs.User;

namespace HomeBudget.Api.Extensions
{
    public static class UserExtensions
    {
        public static void FromUpdateRequest(this User user, UserUpdateRequestModel requestModel)
        {
            user.FirstName = requestModel.FirstName;
            user.LastName = requestModel.LastName;
            user.IsAccountConfirmed = requestModel.IsAccountConfirmed;
            user.IsAccountSetup = requestModel.IsAccountSetup;
            user.Email = requestModel.Email;
            user.PhoneNumber = requestModel.PhoneNumber;
        }

        public static UserGetResponseModel ToGetResponse(this User user)
        {
            return new UserGetResponseModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAccountConfirmed = user.IsAccountConfirmed,
                IsAccountSetup = user.IsAccountSetup,
                Email = string.IsNullOrEmpty(user.Email) ? string.Empty : user.Email,
                PhoneNumber = string.IsNullOrEmpty(user.PhoneNumber) ? string.Empty : user.PhoneNumber
            };
        }

        public static UserUpdateResponseModel ToUpdateResponse(this User user)
        {
            return new UserUpdateResponseModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAccountConfirmed = user.IsAccountConfirmed,
                IsAccountSetup = user.IsAccountSetup,
                Email = string.IsNullOrEmpty(user.Email) ? string.Empty : user.Email,
                PhoneNumber = string.IsNullOrEmpty(user.PhoneNumber) ? string.Empty : user.PhoneNumber
            };
        }
    }
}
