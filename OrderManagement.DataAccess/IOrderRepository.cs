using OrderManagement.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.DataAccess
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<Order> GetByIdAsync(int orderId);
        Task UpdateAsync(int orderId);
        Task DeleteAsync(int orderId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<List<int>> GetAllOrderIdsAsync();
    }
}
