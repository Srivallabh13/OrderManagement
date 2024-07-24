using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.DataAccess.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly OrderDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(OrderDbContext db, ILogger<UserRepository> logger, UserManager<User> userManager)
        {
            _context = db;
            _logger = logger;
            _userManager = userManager;
        }

        public UserRepository() { }

        public async Task DeleteAsync(string userId)
        {
            try
            {
                User user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {userId} not found.");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the user with ID: {userId}.");
                throw new Exception("An error occurred while deleting the user.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all users.");
                throw new Exception("An error occurred while fetching all users.", ex);
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the user with email: {email}.");
                throw new Exception("An error occurred while fetching the user by email.", ex);
            }
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            try
            {
                return await _context.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the user with ID: {userId}.");
                throw new Exception("An error occurred while fetching the user by ID.", ex);
            }
        }

        public async Task<User> UpdateAsync(string id, UpdateUserDTO user)
        {
            try
            {
                User UserToUpdate = await GetByIdAsync(id);
                if (UserToUpdate == null)
                {
                    throw new KeyNotFoundException($"User with ID {id} not found.");
                }

                if (user.Address != null && user.Address.Length > 0)
                    UserToUpdate.Address = user.Address;
                if (user.PinCode != 0 && user.PinCode > 0)
                    UserToUpdate.PinCode = user.PinCode;
                if (user.PhoneNumber != null && user.PhoneNumber.Length > 0)
                    UserToUpdate.PhoneNumber = user.PhoneNumber;
                if (user.UserName != null && user.UserName.Length > 0)
                    UserToUpdate.UserName = user.UserName;
                if (user.ImageUrl != null && user.ImageUrl.Length > 0)
                    UserToUpdate.ImageUrl = user.ImageUrl;
                if (user.City != null && user.City.Length > 0)
                    UserToUpdate.City = user.City;
                if (user.FullName != null && user.FullName.Length > 0)
                    UserToUpdate.FullName = user.FullName;

                _context.Users.Update(UserToUpdate);
                await _context.SaveChangesAsync();
                return UserToUpdate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the user with ID: {id}.");
                throw new Exception("An error occurred while updating the user.", ex);
            }
        }

        public async Task<bool> UpdatePasswordAsync(string id, UpdatePasswordDTO model)
        {
            if (model == null || string.IsNullOrEmpty(id))
            {
                throw new Exception("Invalid input.");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            try
            {
                var verifyResult = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (!verifyResult)
                {
                    return false;
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return true;
                }

                return false;
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Invalid password hash format for user with ID {UserId}", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating the password for user with ID {UserId}", id);
                return false;
            }

        }

        public async Task UpdateRoleAsync(string id, string role)
        {
            var user = await GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("user not found");
            }
            user.Role = role;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
