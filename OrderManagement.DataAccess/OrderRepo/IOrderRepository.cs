using OrderManagement.DomainLayer.Entities;
namespace OrderManagement.DataAccess.OrderRepo
{
    internal interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<Order> GetByIdAsync(Guid orderId);
        Task UpdateAsync(Guid id, string status);
        Task DeleteOrderByUserId(string userId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task DeleteAsync(Guid id);
        Task<User> GetUserAsync(string userId);
        Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId);
    }
}
