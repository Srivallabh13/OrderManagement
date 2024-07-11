using MediatR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using OrderManagement.DataAccess.Email;
using OrderManagement.DataAccess.OrderRepo;
using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.ApplicationLayer.MediatR
{
    public class CreateOrder
    {
        public class Command : IRequest<Order>
        {
            public OrderDTO Order { get; set; }
            public Command(OrderDTO order)
            {
                Order = order;
            }
        }
        public class Handler : IRequestHandler<Command, Order>
        {
            private readonly OrderRepository _orderRepository;
            private readonly EmailSender _emailSender;

            public Handler(OrderRepository orderRepository, EmailSender emailSender)
            {
                _orderRepository = orderRepository;
                _emailSender = emailSender;
            }
            public async Task<Order> Handle(Command request, CancellationToken cancellationToken)
            {

                //bool isAvailable = await _inventoryService.IsProductAvailableAsync(order.ProductName, order.Quantity);
                //if (!isAvailable)
                //{
                //    throw new InvalidOperationException("Product is not available in the required quantity.");
                try
                {
                    OrderDTO _order = request.Order;
                    User user = await _orderRepository.GetUserAsync(_order.CustId);
                    if (user == null)
                    {
                        throw new ArgumentException("User does not exist.");
                    }

                    Order order = CreateOrderObject(_order, user);
                    order.User.Orders.Add(order);
                    await _orderRepository.AddAsync(order);

                    string message = "Thank you for ordering from our platform, Your order is confirmed!";
                    await _emailSender.SendEmailAsync(user.Email, "Order Confirmed", message);

                    return order;
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while creating the order. {ex.Message}");
                }

                //await _inventoryService.UpdateStockAsync(order.ProductName, -order.Quantity);
                //await _emailService.SendOrderConfirmationAsync(createdOrder);
                //return createdOrder;
            }
            private Order CreateOrderObject(OrderDTO _order, User user)
            {
                Order order = new Order
                {
                    Id = _order.Id,
                    CustId = _order.CustId,
                    Status = "Confirmed",
                    Price = _order.Price,
                    Date = DateTime.Now,
                    User = user
                };

                if (order.Products == null)
                {
                    order.Products = new List<OrderProduct>();
                    _order.Products.ForEach(prod =>
                    {
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

                return order;
            }
        }
    }
}