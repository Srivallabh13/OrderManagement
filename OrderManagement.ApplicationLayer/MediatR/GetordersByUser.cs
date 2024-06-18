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
    public class GetordersByUser
    {
        public class Query : IRequest<IEnumerable<Order>>
        {
            public int userId { get; set; }
            public Query(int id)
            {
                this.userId = id;
            }
        }
        public class Handler : IRequestHandler<Query, IEnumerable<Order>>
        {
            private readonly UserRepository _userRepository;
            public Handler(UserRepository userRepository)
            {
                _userRepository = userRepository;
            }
            public async Task<IEnumerable<Order>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _userRepository.GetOrdersByUserAsync(request.userId);
            }
        }

    }
}
