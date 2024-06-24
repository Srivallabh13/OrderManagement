using MediatR;
using OrderManagement.DataAccess;
using OrderManagement.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer.UserMediatR
{
    public class UpdateUser
    {
        public class Command: IRequest<Unit>
        {
            public User user { get; }
            public Command(User user)
            {
                this.user = user;
            }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            public UserRepository _userRepository { get; }
            public Handler(UserRepository userRepository)
            {
                _userRepository = userRepository;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _userRepository.UpdateAsync(request.user);
                return Unit.Value;
            }
        }
    }
}
