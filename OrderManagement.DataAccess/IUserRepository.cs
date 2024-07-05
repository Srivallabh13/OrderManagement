﻿using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.DataAccess
{
    internal interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User> GetByIdAsync(int userId);
        Task UpdateAsync(User user);                      // Update
        Task DeleteAsync(int userId);                     // Delete
        Task<IEnumerable<User>> GetAllAsync();            // Read
        Task<User> GetByEmailAsync(string email);         // Read by emai
    }
}
