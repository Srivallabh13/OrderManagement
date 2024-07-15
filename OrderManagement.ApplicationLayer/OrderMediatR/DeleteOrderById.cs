using MediatR;
using OrderManagement.DataAccess.OrderRepo;

namespace OrderManagement.ApplicationLayer.MediatR
{
    public class DeleteOrderById
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }
            public Command(int id)
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
                int id = request.Id;
                try
                {
                    await _orderRepository.DeleteAsync(id);
                    return Unit.Value;
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while deleting the order with ID: {id}, {ex.Message} .");
                }
            }

        }
    }
}
