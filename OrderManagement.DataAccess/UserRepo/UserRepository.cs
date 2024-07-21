using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.DataAccess.UserRepo
{
    public class UserRepository : IUserRepository
    {
        readonly OrderDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(OrderDbContext db, UserManager<User> userManager, ILogger<UserRepository> logger)
        {
            _context = db;
            _userManager = userManager;
            _logger = logger;
        }
        public UserRepository() { }
        public async Task DeleteAsync(string userId)
        {
            User user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);

        }

        public async Task<User> UpdateAsync(string id, UpdateUserDTO user)
        {
            User UserToUpdate = await GetByIdAsync(id);
            if (UserToUpdate != null)
            {
                if (user.Address != null && user.Address.Length > 0)
                    UserToUpdate.Address = user.Address;
                if (user.Email != null && user.Email.Length > 0)
                    UserToUpdate.Email = user.Email;
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
            else
            {
                throw new Exception("No User Found");
            }
        }

        public async Task<bool> UpdatePasswordAsync(string id, UpdatePasswordDTO model)
        {
            // Check if model is null
            if (model == null || string.IsNullOrEmpty(id))
            {
                throw new Exception( "Invalid input.");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new Exception( "User not found.");
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