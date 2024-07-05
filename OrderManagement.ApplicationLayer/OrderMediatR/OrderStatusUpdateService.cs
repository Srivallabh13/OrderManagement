using MediatR;
using Microsoft.Extensions.Hosting;
using OrderManagement.DataAccess.OrderRepo;

namespace OrderManagement.ApplicationLayer.MediatR
{
    public class OrderStatusUpdateService : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;
        private readonly TimeSpan _updateInterval = TimeSpan.FromHours(24);

        public OrderStatusUpdateService(IMediator mediator, IOrderRepository orderRepository)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    await Task.Delay(_updateInterval, stoppingToken);

            //    // Fetch all order IDs that need status update (if any filtering logic is required)
            //    var allOrderIds = await _orderRepository.GetAllOrderIdsAsync();

            //    // Process each order to update its status
            //    foreach (var orderId in allOrderIds)
            //    {
            //        var command = new UpdateOrderStatusById.Command(orderId,"");
            //        await _mediator.Send(command, stoppingToken);
            //    }
            //}
        }
    }
}
