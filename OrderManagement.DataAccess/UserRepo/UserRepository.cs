using Microsoft.EntityFrameworkCore;
using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.DataAccess.UserRepo
{
    public class UserRepository : IUserRepository
    {
        readonly OrderDbContext _context;
        public UserRepository(OrderDbContext db)
        {
            _context = db;
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
                if(user.Address!=null && user.Address.Length>0)
                    UserToUpdate.Address = user.Address;
                if(user.PinCode!=0 && user.PinCode>0)
                    UserToUpdate.PinCode = user.PinCode;
                if(user.PhoneNumber != null && user.PhoneNumber.Length > 0)
                    UserToUpdate.PhoneNumber = user.PhoneNumber;
                if(user.UserName != null && user.UserName.Length > 0)
                    UserToUpdate.UserName = user.UserName;
                if(user.ImageUrl != null && user.ImageUrl.Length > 0)
                    UserToUpdate.ImageUrl = user.ImageUrl;
                if(user.City != null && user.City.Length > 0)
                    UserToUpdate.City = user.City;
                if(user.FullName != null && user.FullName.Length > 0 )
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
    }
}
