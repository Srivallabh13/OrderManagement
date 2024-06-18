using MediatR;
using OrderManagement.DataAccess;
using OrderManagement.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer.MediatR
{
    public class CreateOrder
    {
        //create command
        public class Command : IRequest<Order>
        {
            public Order Order { get; set; }

            public Command(Order order)
            {
                Order = order;
            }

        }
        public class Handler : IRequestHandler<Command, Order>
        {
            private readonly IOrderRepository _orderRepository;
            public Handler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }
            public async Task<Order> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = request.Order;

                //bool isAvailable = await _inventoryService.IsProductAvailableAsync(order.ProductName, order.Quantity);
                //if (!isAvailable)
                //{
                //    throw new InvalidOperationException("Product is not available in the required quantity.");
                order.Date = DateTime.Now;
                order.Status = "Confirmed";
                var createdOrder = await _orderRepository.AddAsync(order);
                //await _inventoryService.UpdateStockAsync(order.ProductName, -order.Quantity);
                //await _emailService.SendOrderConfirmationAsync(createdOrder);

                return createdOrder;
            }

        }
    }
}
