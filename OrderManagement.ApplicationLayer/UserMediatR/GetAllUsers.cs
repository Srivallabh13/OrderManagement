using MediatR;
using OrderManagement.DataAccess.UserRepo;
using OrderManagement.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer.UserMediatR
{
    public class GetAllUsers
    {
        public class Query : IRequest<IEnumerable<User>> 
        { 

        }

        public class Handler : IRequestHandler<Query, IEnumerable<User>>
        {
            private readonly UserRepository _userRepository;

            public Handler(UserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<IEnumerable<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _userRepository.GetAllAsync();
            }
        }

    }
}
