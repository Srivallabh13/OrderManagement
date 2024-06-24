using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.ApplicationLayer
{
    internal interface IOrderService
    {
        // create order
        //create and add to the DB. -> call AddAsync. -> -> product,stock ->  valid -> avail -> call inventory update -> email service (trigger) 
        Task<Order> CreateOrderAsync(OrderDTO order);
        Task DeleteOrderByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByUserAsync(string orderId);
        Task UpdateOrderStatusById(Guid id);
    }
}
