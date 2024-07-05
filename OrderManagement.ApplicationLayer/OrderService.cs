using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
                    Status = "Confirmed",
                    Price = _order.Price,
                    Date = DateTime.Now,
                    User = user
                };
                if(order.Products == null)
                {
                    order.Products = new List<OrderProduct>();
                    _order.Products.ForEach(prod => {
                    
                        OrderProduct orderProduct = new OrderProduct
                        {
                            Id = _order.Id,
                            Quantity = prod.Quantity,
                            ProductId = prod.Id
                        };
                        order.Products.Add(orderProduct);
                    });
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
                //create an order
                return order;
            }
            else
            {
                throw new ArgumentException("User does not exist.");
            }
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

        public async Task UpdateOrderStatusById(Guid id, string status)
    {
    }
}
