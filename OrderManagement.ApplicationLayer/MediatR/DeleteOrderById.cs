using MediatR;
using OrderManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer.MediatR
{
    public class DeleteOrderById
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }
            public Command( int id)
            {
                this.Id = id;
            }
        }
        public class Handler : IRequestHandler<Command,Unit>
        {
            private readonly OrderRepository _orderRepository;
            public Handler(OrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _orderRepository.DeleteAsync(request.Id);
                return Unit.Value;
            }

        }
    }
}
