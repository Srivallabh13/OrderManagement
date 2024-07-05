using MediatR;
using OrderManagement.DataAccess;
using OrderManagement.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer.MediatR
{
    public class GetOrdersByUser
    {
        public class Query : IRequest<IEnumerable<Order>>
        {
            public string userId { get; set; }
            public Query(string id)
            {
                this.userId = id;
            }
        }
        public class Handler : IRequestHandler<Query, IEnumerable<Order>>
        {
            private readonly OrderService _orderService;
            public Handler(OrderService orderService)
            {
                _orderService = orderService;
            }
            public async Task<IEnumerable<Order>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _orderService.GetOrdersByUserAsync(request.userId);
            }
        }
    }
}
