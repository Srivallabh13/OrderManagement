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
            public Guid Id { get; set; }
            public Command(Guid id)
            {
                this.Id = id;
            }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly OrderService _orderService;
            public Handler(OrderService orderService)
            {
                _orderService = orderService;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _orderService.DeleteOrderByIdAsync(request.Id);
                return Unit.Value;
            }

        }
    }
}
