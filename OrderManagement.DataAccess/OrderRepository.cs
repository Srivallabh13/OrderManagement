using Microsoft.EntityFrameworkCore;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.DataAccess
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

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();

        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task UpdateAsync(Order order)
        {
             _context.Orders.Update(order);
            await _context.SaveChangesAsync();
           
        }

        
    }
}
