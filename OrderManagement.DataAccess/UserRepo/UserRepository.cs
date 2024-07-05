using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId)
        {
            User user = _context.Users.Find(userId);
            return user.Orders;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
