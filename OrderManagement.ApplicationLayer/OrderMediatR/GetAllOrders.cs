using MediatR;
using OrderManagement.DataAccess;
using OrderManagement.DataAccess.OrderRepo;
using OrderManagement.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer.MediatR
{
    public class GetAllOrders
    {
        public class Query : IRequest<IEnumerable<Order>>
        {

        }
        public class Handler : IRequestHandler<Query, IEnumerable<Order>>
        {
            private readonly OrderRepository _orderRepository;
            public Handler(OrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }
            public async Task<IEnumerable<Order>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _orderRepository.GetAllAsync();
            }
        }
    }
}
