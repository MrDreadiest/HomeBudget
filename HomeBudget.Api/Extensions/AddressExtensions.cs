using HomeBudget.Api.Entities;
using HomeBudget.Common.EntityDTOs.Address;

namespace HomeBudget.Api.Extensions
{
    public static class AddressExtensions
    {
        public static void FromUpdateRequest(this Address address, AddressUpdateRequestModel requestModel)
        {
            address.Street = requestModel.Street;
            address.HouseNumber = requestModel.HouseNumber;
            address.ApartmentNumber = requestModel.ApartmentNumber;
            address.City = requestModel.City;
            address.Region = requestModel.Region;
            address.PostalCode = requestModel.PostalCode;
            address.Country = requestModel.Country;
        }

        public static AddressGetResponseModel ToGetResponse(this Address address)
        {
            return new AddressGetResponseModel
            {
                Id = address.Id,
                Street = address.Street,
                HouseNumber = address.HouseNumber,
                ApartmentNumber = address.ApartmentNumber,
                City = address.City,
                Region = address.Region,
                PostalCode = address.PostalCode,
                Country = address.Country
            };
        }

        public static AddressUpdateResponseModel ToUpdateResponse(this Address address)
        {
            return new AddressUpdateResponseModel
            {
                Id = address.Id,
                Street = address.Street,
                HouseNumber = address.HouseNumber,
                ApartmentNumber = address.ApartmentNumber,
                City = address.City,
                Region = address.Region,
                PostalCode = address.PostalCode,
                Country = address.Country
            };
        }
    }
}
