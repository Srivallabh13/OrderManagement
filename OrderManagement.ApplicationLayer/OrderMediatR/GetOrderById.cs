using MediatR;
using OrderManagement.DomainLayer.Entities;
namespace OrderManagement.ApplicationLayer.MediatR
{
    public class GetOrderById
    {
        public class Query : IRequest<Order>
        {
            public Guid Id { get; set; }
            public Query(Guid id)
            {
                Id = id;
            }
        }
        public class Handler : IRequestHandler<Query, Order>
        {
            private readonly OrderService _orderService;
            public Handler(OrderService orderService)
            {
                _orderService = orderService;
            }
            public async Task<Order> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _orderService.GetOrderByIdAsync(request.Id);
            }
        }
    }
}
