using MediatR;
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
            private readonly OrderService _orderService;
            public Handler(OrderService orderService)
            {
                _orderService = orderService;
            }
            public async Task<Order> Handle(Command request, CancellationToken cancellationToken)
            {

                //bool isAvailable = await _inventoryService.IsProductAvailableAsync(order.ProductName, order.Quantity);
                //if (!isAvailable)
                //{
                //    throw new InvalidOperationException("Product is not available in the required quantity.");
                var createdOrder = await _orderService.CreateOrderAsync(request.Order);
                //await _inventoryService.UpdateStockAsync(order.ProductName, -order.Quantity);
                //await _emailService.SendOrderConfirmationAsync(createdOrder);

                return createdOrder;
            }
        }
    }
}