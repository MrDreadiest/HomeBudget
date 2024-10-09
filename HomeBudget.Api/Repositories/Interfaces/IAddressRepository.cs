using HomeBudget.Api.Entities;

namespace HomeBudget.Api.Repositories.Interfaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address?> GetAddressByIdAsync(string addressId);
    }
}
