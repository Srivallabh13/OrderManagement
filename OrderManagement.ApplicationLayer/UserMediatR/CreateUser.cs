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
    public class CreateUser
    {
        public class Command : IRequest<User>
        {
            public User User { get; set; }
            public Command(User user)
            {
                User = user;
            }
        }

        public class Handler : IRequestHandler<Command, User>
        {
            private readonly UserRepository _userRepository;

            public Handler(UserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                return _userRepository.AddAsync(request.User);
            }
        }
    }
}
