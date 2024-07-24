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
            private readonly OrderRepository _orderRepository;
            public Handler(OrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }
            public async Task<IEnumerable<Order>> Handle(Query request, CancellationToken cancellationToken)
            {
                string userId = request.userId;
                try
                {
                    IEnumerable<Order> orders = await _orderRepository.GetOrdersByUserAsync(userId);

                    return orders.Where(order => order.CustId == userId);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"An error occurred while fetching orders for user ID: {userId},{ex.Message}", ex);
                }
            }
        }
    }
}
