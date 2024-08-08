using MediatR;
using Microsoft.Identity.Client;
using OrderManagement.ApplicationLayer.Photos.Interfaces;
using OrderManagement.ApplicationLayer.Photos.PhotoMediatR;
using OrderManagement.DataAccess.OrderRepo;
using OrderManagement.DataAccess.UserRepo;
using OrderManagement.DomainLayer.Entities;
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
            private readonly IPhotoAccessor photoAccessor;
            private readonly OrderRepository _orderRepository;

            public Handler(UserRepository userRepository, IPhotoAccessor photoAccessor, OrderRepository orderRepository)
            {
                _userRepository = userRepository;
                this.photoAccessor = photoAccessor;
                _orderRepository = orderRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(request.Id);
                if(user.Photos.Count>0)
                {
                    string result = null;
                    foreach (Photo photo in user.Photos)
                    {
                        result = photoAccessor.DeletePhotoFromCloudinary(photo.Id).ToString();
                    }
                    if(result != null)
                    {
                        if (user?.Orders != null || user?.Orders?.Count != 0)
                        {
                            await _orderRepository.DeleteOrderByUserId(request.Id);
                        }
                        await _userRepository.DeleteAsync(request.Id);
                        await _userRepository.DeleteAsync(request.Id);
                        return Unit.Value;
                    }
                }
                else
                {
                    if (user?.Orders != null || user?.Orders?.Count != 0)
                    {
                        await _orderRepository.DeleteOrderByUserId(request.Id);
                    }
                    await _userRepository.DeleteAsync(request.Id);
                }
                    return Unit.Value;
            }
        }
    }
}
