using MediatR;
using Microsoft.Identity.Client;
using OrderManagement.DataAccess;
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

            public Handler(UserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _userRepository.DeleteAsync(request.Id);
                return Unit.Value;
            }
        }
    }
}
