using MediatR;
using OrderManagement.DataAccess.OrderRepo;
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
            private readonly OrderRepository _orderRepository;
            public Handler(OrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }
            public async Task<Order> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _orderRepository.GetByIdAsync(request.Id);
            }
        }
    }
}
