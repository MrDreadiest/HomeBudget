using HomeBudget.Common.EntityDTOs.Address;

namespace HomeBudget.Api.Services.Interfaces
{
    public interface IAddressService
    {
        Task<AddressGetResponseModel?> GetAddressByIdsAsync(string userId, string addressId);
        Task<AddressUpdateResponseModel?> UpdateAddressAsync(string userId, string addressId, AddressUpdateRequestModel requestModel);
    }
}