using OrderManagement.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.DataAccess.UserRepo
{
    internal interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User> GetByIdAsync(string userId);
        Task UpdateAsync(User user);
        Task DeleteAsync(string userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByEmailAsync(string email);

    }
}
