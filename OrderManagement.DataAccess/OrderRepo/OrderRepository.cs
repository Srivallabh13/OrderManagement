using Microsoft.EntityFrameworkCore;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.DataAccess.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext db)
        {
            _context = db;
        }

        public async Task<Order> AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            _context.SaveChanges();
            return order;
        }

        public async Task DeleteAsync(Guid id)
        {
            Order order = await _context.Orders
                                .Include(o => o.Products)
                                .FirstOrDefaultAsync(o => o.Id == id);
            if (order != null)
            {
                _context.OrderProducts.RemoveRange(order.Products);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Order Not found");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.Products).ToListAsync();

        }

        public async Task<Order> GetByIdAsync(Guid orderId)
        {
            return await _context.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId)
        {
            User user = await _context.Users.FindAsync(userId);
            return user.Orders;
        }

        public async Task<User> GetUserAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task UpdateAsync(Guid orderId, string status)
        {
            Order order = await _context.Orders.FindAsync(orderId);
            order.Status = status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}