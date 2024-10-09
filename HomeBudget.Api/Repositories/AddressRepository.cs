using HomeBudget.Api.Data;
using HomeBudget.Api.Entities;
using HomeBudget.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Api.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly DataContext _context;

        public AddressRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Address?> GetAddressByIdAsync(string addressId)
        {
            return await _context.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id.Equals(addressId));
        }

        public async Task AddAsync(Address address)
        {
            await Task.CompletedTask;
        }

        public void Delete(Address address)
        {
            _context.Addresses.Remove(address);
        }

        public void Update(Address address)
        {
            _context.Addresses.Update(address);
        }
    }
}
