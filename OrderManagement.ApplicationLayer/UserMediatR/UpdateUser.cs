using MediatR;
using OrderManagement.DataAccess.UserRepo;
using OrderManagement.DomainLayer.DTO;
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
        public class Command: IRequest<User>
        {
            public UpdateUserDTO user { get; }
            public string id { get; }
            public Command(string id, UpdateUserDTO user)
            {
                this.user = user;
                this.id = id;
            }
        }
        public class Handler : IRequestHandler<Command, User>
        {
            public UserRepository _userRepository { get; }
            public Handler(UserRepository userRepository)
            {
                _userRepository = userRepository;
            }
            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                 return await _userRepository.UpdateAsync(request.id, request.user);
            }
        }
    }
}
