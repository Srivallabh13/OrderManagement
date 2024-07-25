using Microsoft.EntityFrameworkCore;
using OrderManagement.DataAccess.Email;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.DataAccess.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        private readonly EmailSender emailSender;

        public OrderRepository(OrderDbContext db, EmailSender emailSender)
        {
            _context = db;
            this.emailSender = emailSender;
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
            User user = await _context.Users.FindAsync(order.CustId);
            order.Status = status;
            string message = $"Your Order with orderId {orderId}, Your order successfully {status}!";
            await emailSender.SendEmailAsync(user.Email, "Order Status", message);
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderByUserId(string userId)
        {
            try
            {
                var orders = await _context.Orders.Include(o => o.Products).Where(o => o.CustId == userId).ToListAsync();

                foreach (var order in orders)
                {
                    _context.OrderProducts.RemoveRange(order.Products);
                    _context.Orders.Remove(order);
                }

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the orders.{ex.Message}");
            }
        }
    }
}