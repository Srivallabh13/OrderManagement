using OrderManagement.DataAccess;
using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.ApplicationLayer
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepo, UserRepository userRepository)
        {
            _orderRepository = orderRepo;
        }
        public async Task<Order> CreateOrderAsync(OrderDTO _order)
        {
            //validate the user details and product availability
            User user = await _orderRepository.GetUserAsync(_order.CustId);
            if(user != null)
            {
                Order order = new Order
                {
                    Id = _order.Id,
                    CustId = _order.CustId,
                    Products = _order.Products,
                    Status = "Confirmed",
                    Price = _order.Price,
                    Date = DateTime.Now,
                };
                if(order.User == null)
                {
                    order.User = new User();
                    order.User = user;
                }
                if (order.User.Orders == null)
                {
                    order.User.Orders = new List<Order>();
                }
                order.User.Orders.Add(order);
                await _orderRepository.AddAsync(order);

                return order;
            }
            //create an order
                throw new ArgumentException("User does not exist.");
        }

        public async Task DeleteOrderByIdAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<string> GetDeliveryStatus(Guid orderId)
        {
            Order o = await _orderRepository.GetByIdAsync(orderId);
            return o.Status;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId)
        {
            IEnumerable<Order> orders = await _orderRepository.GetAllAsync();
            
            return orders.Where(order=> order.CustId == userId);
        }

        public async Task UpdateOrderStatusById(Guid id)
        {
            await _orderRepository.UpdateAsync(id);
        }
    }
}
