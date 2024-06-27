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
    public class GetUserById
    {
        public class Query : IRequest<User>
        {
        public string Id { get; }
            public Query(string id)
            {
                Id = id;
            }
        }
        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserRepository _userRepository;

            public Handler(UserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                return _userRepository.GetByIdAsync(request.Id);
            }
        }
    }
}
