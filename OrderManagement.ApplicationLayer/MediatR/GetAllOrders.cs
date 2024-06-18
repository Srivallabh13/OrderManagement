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
    public class GetAllOrders
    {
        public class Query : IRequest<IEnumerable<Order>>
        {

        }
        public class Handler : IRequestHandler<Query, IEnumerable<Order>>
        {
            private readonly IOrderRepository _orderRepository;
            public Handler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }
            //public  async Task<Order> Handle(Query request, CancellationToken cancellationToken)
            //{
            //}
            public async Task<IEnumerable<Order>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _orderRepository.GetAllAsync();
            }
        }
    }
}
