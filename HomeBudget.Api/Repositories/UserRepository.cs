using HomeBudget.Api.Data;
using HomeBudget.Api.Entities;
using HomeBudget.Api.Repositories.Interfaces;

namespace HomeBudget.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
        }
    }
}
