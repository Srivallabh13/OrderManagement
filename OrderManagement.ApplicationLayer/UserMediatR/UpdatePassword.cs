using MediatR;
using OrderManagement.DataAccess.UserRepo;
using OrderManagement.DomainLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer.UserMediatR
{
    public class UpdatePassword
    {
        public class Command : IRequest<bool>
        {
            public string Id { get; set; }
            public UpdatePasswordDTO updatePasswordDTO { get; set; }

            public Command(string Id, UpdatePasswordDTO updatePasswordDTO)
            {
                this.Id = Id;
                this.updatePasswordDTO = updatePasswordDTO;
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            public UserRepository _userRepository { get; }
            public Handler(UserRepository userRepository)
            {
                _userRepository = userRepository;
            }
            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _userRepository.UpdatePasswordAsync(request.Id, request.updatePasswordDTO);
            }
        }
    }
}