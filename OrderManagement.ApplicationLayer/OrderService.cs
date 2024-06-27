using OrderManagement.DataAccess.Email;
using OrderManagement.DataAccess.OrderRepo;
using OrderManagement.DataAccess.UserRepo;
using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.ApplicationLayer
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly EmailSender emailSender;

        public OrderService(OrderRepository orderRepo, EmailSender emailSender)
        {
            _orderRepository = orderRepo;
            this.emailSender = emailSender;
        }
        public async Task<Order> CreateOrderAsync(OrderDTO _order)
        {
            //validate the user details and product availability
            User user = await _orderRepository.GetUserAsync(_order.CustId);
            if(user != null)
            {
                // Product Inventroy code.
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

                //email service 
                string message = "Thank you for ordering from our platform, Your order is confirmed!";
                await emailSender.SendEmailAsync(user.Email, "Order Confirmed", message);
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
