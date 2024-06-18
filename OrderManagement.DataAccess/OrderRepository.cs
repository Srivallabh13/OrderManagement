using Microsoft.EntityFrameworkCore;
using OrderManagement.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task DeleteAsync(int orderId)
        {
            Order o = await _context.Orders.FindAsync(orderId);
            if (o != null)
            {
                _context.Orders.Remove(o);
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();

        }

        public async Task<List<int>> GetAllOrderIdsAsync()
        {
            return await _context.Orders
                                 .Select(o=> o.Id)
                                 .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task UpdateAsync(int orderId)
        {
            Order order = await _context.Orders.FindAsync(orderId);
             _context.Orders.Update(order);
            await _context.SaveChangesAsync();
           
        }

        
    }
}
