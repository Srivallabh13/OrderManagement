using MediatR;
using Microsoft.Identity.Client;
using OrderManagement.DataAccess.OrderRepo;
using OrderManagement.DataAccess.UserRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer.UserMediatR
{
    public class DeleteUser
    {
        public class Command : IRequest<Unit>
        {
            public string Id { get; }
            public Command(string id)
            {
                Id = id;
            }

        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly UserRepository _userRepository;
            private readonly OrderRepository _orderRepository;
            public Handler(UserRepository userRepository, OrderRepository orderRepository)
            {
                _userRepository = userRepository;
                _orderRepository = orderRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(request.Id);
                if (user?.Orders != null || user?.Orders?.Count!=0) { 
                    await _orderRepository.DeleteOrderByUserId(request.Id);
                }
                await _userRepository.DeleteAsync(request.Id);
                return Unit.Value;
            }
        }
    }
}
