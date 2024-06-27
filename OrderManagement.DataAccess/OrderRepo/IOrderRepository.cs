using OrderManagement.DomainLayer.Entities;
namespace OrderManagement.DataAccess.OrderRepo
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<Order> GetByIdAsync(Guid orderId);
        Task UpdateAsync(Guid id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task DeleteAsync(Guid id);
        Task<List<Guid>> GetAllOrderIdsAsync();
        Task<User> GetUserAsync(string userId);
        Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId);
    }
}
