using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagement.DataAccess.Email;
using OrderManagement.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.DataAccess.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        private readonly EmailSender emailSender;

        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(OrderDbContext db, EmailSender emailSender, ILogger<OrderRepository> logger)
        {
            _context = db;
            this.emailSender = emailSender;
            _logger = logger;
        }
        
        public async Task<Order> AddAsync(Order order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding an order.");
                throw new Exception($"An error occurred while adding the order.{ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                Order order = await _context.Orders
                                    .Include(o => o.Products)
                                    .FirstOrDefaultAsync(o => o.Id == id);
                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                _context.OrderProducts.RemoveRange(order.Products);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the order with ID: {id}.");
                throw new Exception($"An error occurred while deleting the order.{ex.Message}", ex);
            }
        }

        public async Task DeleteOrderByUserId(string userId)
        {
            try
            {
                // Fetch all orders related to the given user ID
                var orders = await _context.Orders
                                    .Include(o => o.Products)
                                    .Where(o => o.CustId == userId)
                                    .ToListAsync();

               
                // Remove related products and orders
                foreach (var order in orders)
                {
                    _context.OrderProducts.RemoveRange(order.Products);
                    _context.Orders.Remove(order);
                }

                await _context.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the orders for user ID: {userId}.");
                throw new Exception($"An error occurred while deleting the orders.{ex.Message}" );
            }
        }
      


        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                return await _context.Orders.Include(o => o.Products).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all orders.");
                throw new Exception($"An error occurred while fetching all orders.{ex.Message}");
            }
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            try
            {
                return await _context.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the order with ID: {orderId}.");
                throw new Exception($"An error occurred while fetching the order.{ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId)
        {
            try
            {
                User user = await _context.Users.Include(u => u.Orders).ThenInclude(o => o.Products).FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                return user.Orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching orders for user ID: {userId}.");
                throw new Exception($"An error occurred while fetching orders for the user.{ex.Message}", ex);
            }
        }

        public async Task<User> GetUserAsync(string userId)
        {
            try
            {
                return await _context.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the user with ID: {userId}.");
                throw new Exception($"An error occurred while fetching the user.{ex.Message}", ex);
            }
        }

        public async Task UpdateAsync(int orderId, string status)
        {
            try
            {
                Order order = await _context.Orders.FindAsync(orderId);
                User user = await _context.Users.FindAsync(order.CustId);

                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                order.Status = status;
                string message = $"Your Order with orderId {orderId}, Your order successfully {status}!";
                await emailSender.SendEmailAsync(user.Email, "Order Status", message);

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the order status for order ID: {orderId}.");
                throw new Exception($"An error occurred while updating the order status.{ex.Message}", ex);
            }
        }

        
    }
}
