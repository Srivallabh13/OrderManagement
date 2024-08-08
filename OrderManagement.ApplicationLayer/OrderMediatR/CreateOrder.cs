using MediatR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
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
            private readonly EmailSender emailSender;
            public Handler(OrderRepository orderRepo, EmailSender emailSender)
            {
                _orderRepository = orderRepo;
                this.emailSender = emailSender;
            }
            public async Task<Order> Handle(Command request, CancellationToken cancellationToken)
            {
                OrderDTO _order = request.Order;
                User user = await _orderRepository.GetUserAsync(_order.CustId);
                if (user != null)
                {
                    // Product Inventroy code.
                    Order order = CreateOrderObject(_order, user);
                    await _orderRepository.AddAsync(order);

                    //email service 
                    string message = $"Thank you {user.FullName}, for ordering from our platform, Your order is confirmed! You have Ordered {order.Products.Count} products. Now you can track you Order with this id: {order.Id}.\nWe will inform you as soon as the order gets shipped via email.\n Keep Shopping!\n For any queries contact us on srivallabhjoshi13@gmail.com";
                    await emailSender.SendEmailAsync(user.Email, "Order Confirmed", message);
                    //create an order
                    return order;
                }
                else
                {
                    throw new ArgumentException("User does not exist.");
                }
            }

            public static Order CreateOrderObject(OrderDTO _order, User user)
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
                order.User.Orders.Add(order);

                return order;
            }
        }
    }
}