using OrderManagement.DomainLayer.Entities;
namespace OrderManagement.DataAccess.OrderRepo
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<Order> GetByIdAsync(int orderId);
        Task UpdateAsync(int id, string status);
        Task<IEnumerable<Order>> GetAllAsync();
        Task DeleteOrderByUserId(string userId);
        Task DeleteAsync(int id);
        Task<User> GetUserAsync(string userId);
        Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId);
    }
}
