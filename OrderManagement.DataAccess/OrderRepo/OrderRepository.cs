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
            Order order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Remove(order);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Order Not found");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return _context.Orders.ToList();

        }

        public Task<List<Guid>> GetAllOrderIdsAsync()
        {
            throw null;
        }

        public async Task<Order> GetByIdAsync(Guid orderId)
        {
            return await _context.Orders.FindAsync(orderId);
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

        public async Task UpdateAsync(Guid orderId)
        {
            // correct implementation left..
            Order order = await _context.Orders.FindAsync(orderId);
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }


    }
}
