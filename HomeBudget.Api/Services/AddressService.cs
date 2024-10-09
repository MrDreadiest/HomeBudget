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

                return address == null ? null : new AddressGetResponseModel() 
                {
                    Id = address.Id,
                    Street = address.Street,
                    HouseNumber = address.HouseNumber,
                    ApartmentNumber = address.ApartmentNumber,
                    City = address.City,
                    Country = address.Country,
                    PostalCode = address.PostalCode,
                    Region = address.Region
                };
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
                        address.Street = requestModel.Street;
                        address.HouseNumber = requestModel.HouseNumber;
                        address.ApartmentNumber = requestModel.ApartmentNumber;
                        address.City = requestModel.City;
                        address.Country = requestModel.Country;
                        address.PostalCode = requestModel.PostalCode;
                        address.Region = requestModel.Region;

                        await _unitOfWork.SaveChangesAsync();

                        return new AddressUpdateResponseModel()
                        {
                            Id = address.Id,
                            Street = address.Street,
                            HouseNumber = address.HouseNumber,
                            ApartmentNumber = address.ApartmentNumber,
                            City = address.City,
                            Country = address.Country,
                            PostalCode = address.PostalCode,
                            Region = address.Region
                        };
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