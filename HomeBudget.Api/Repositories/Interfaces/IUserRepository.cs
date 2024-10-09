using HomeBudget.Api.Entities;

namespace HomeBudget.Api.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByIdAsync(string id);
    }
}
