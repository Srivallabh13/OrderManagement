using MediatR;
using OrderManagement.DataAccess.OrderRepo;

namespace OrderManagement.ApplicationLayer.MediatR
{
    public class DeleteOrderById
    {
        public class Command : IRequest<Unit>
        {
            public Guid Id { get; set; }
            public Command(Guid id)
            {
                this.Id = id;
            }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly OrderRepository _orderRepository;
            public Handler(OrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //await _orderService.DeleteOrderByIdAsync(request.Id);
                await _orderRepository.DeleteAsync(request.Id);
                return Unit.Value;
            }

        }
    }
}
