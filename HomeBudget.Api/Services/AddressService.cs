using HomeBudget.Api.Extensions;
using HomeBudget.Api.Services.Interfaces;
using HomeBudget.Api.UnitOfWork.Interfaces;
using HomeBudget.Common.EntityDTOs.Address;

namespace HomeBudget.Api.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AddressGetResponseModel?> GetAddressByIdsAsync(string userId, string addressId)
        {
            if (await HasAccessToAddressAsync(userId, addressId))
            {
                var address = await _unitOfWork.Addresses.GetAddressByIdAsync(addressId);

                return address == null ? null : address.ToGetResponse();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<AddressUpdateResponseModel?> UpdateAddressAsync(string userId, string addressId, AddressUpdateRequestModel requestModel)
        {
            if (await HasAccessToAddressAsync(userId, addressId))
            {
                var address = await _unitOfWork.Addresses.GetAddressByIdAsync(addressId);

                if (address != null && requestModel != null) 
                {
                    if (address.IsAddressValid())
                    {
                        address.FromUpdateRequest(requestModel);

                        await _unitOfWork.SaveChangesAsync();

                        return address.ToUpdateResponse();  
                    }
                }
                return null;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        private async Task<bool> HasAccessToAddressAsync(string userId, string addressId)
        {
            var address = await _unitOfWork.Addresses.GetAddressByIdAsync(addressId);
            return address != null && address.User.Id.Equals(userId);
        }
    }
}