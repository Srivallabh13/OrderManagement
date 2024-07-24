using MediatR;
using OrderManagement.DataAccess.UserRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer.UserMediatR
{
    public class UpdateRole
    {
        public class Command : IRequest<Unit>
        {
            public string Id { get; set; }
            public string Role { get; set; }
            public Command(string _id, string _role)
            {
                Id = _id;
                Role = _role;
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
                await _userRepository.UpdateRoleAsync(request.Id, request.Role);
                return Unit.Value;
            }
        }
    }
}