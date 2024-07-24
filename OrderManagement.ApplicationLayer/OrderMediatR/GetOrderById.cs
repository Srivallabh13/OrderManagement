using MediatR;
using OrderManagement.DataAccess.OrderRepo;
using OrderManagement.DomainLayer.Entities;
namespace OrderManagement.ApplicationLayer.MediatR
{
    public class GetOrderById
    {
        public class Query : IRequest<Order>
        {
            public int Id { get; set; }
            public Query(int id)
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
                try
                {
                    return await _orderRepository.GetByIdAsync(request.Id);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while fetching the order with ID: {request.Id},{ex.Message}");
                }
            }
        }
    }
}
