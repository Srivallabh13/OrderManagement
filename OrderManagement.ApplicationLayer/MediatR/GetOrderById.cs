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
            private readonly IOrderRepository _orderRepository;
            public Handler(IOrderRepository orderRepository)
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
