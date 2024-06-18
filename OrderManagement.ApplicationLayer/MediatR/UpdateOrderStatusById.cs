using MediatR;
using OrderManagement.DataAccess;
using OrderManagement.DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer.MediatR
{
    public class UpdateOrderStatusById
    {
        public class command : IRequest<Unit>
        {
            

            public int Id { get; set; }
            public command(int id)
            {
                Id = id;
            }

            
        }
        public class Handler : IRequestHandler<command, Unit>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly TimeSpan _updateInterval = TimeSpan.FromHours(24);

            public Handler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }
            public async Task<Unit> Handle(command request, CancellationToken cancellationToken)
            {
                var order = await _orderRepository.GetByIdAsync(request.Id);

                // functionality that updates order status for evry 24hrs
                if (order != null)
                {
                    // Calculate the time elapsed since order creation
                    var timeElapsed = DateTime.UtcNow - order.Date;

                    // Check if it's time to update the status (24 hours or more since creation)
                    if (timeElapsed >= _updateInterval)
                    {
                        // Update the order status based on the current status
                        switch (order.Status)
                        {
                            case "Confirmed":
                                order.Status = "Shipped";
                                break;
                            case "Shipped":
                                order.Status = "Dispatched";
                                break;
                            case "Dispatched":
                                order.Status = "Delivered";
                                break;

                            default:
                               
                                break;
                        }

                        // Save changes to the repository
                        await _orderRepository.UpdateAsync(request.Id);
                    }
                }
                return Unit.Value;
            }
        }

        
    }
}
