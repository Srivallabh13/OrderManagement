using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.DataAccess.UserRepo
{
    internal interface IUserRepository
    {
        Task<User> GetByIdAsync(string userId);
        Task<User> UpdateAsync(string id,UpdateUserDTO user);
        Task DeleteAsync(string userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByEmailAsync(string email);

    }
}
