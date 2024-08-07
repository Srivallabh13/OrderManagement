﻿using Azure.Core;
using MediatR;
using OrderManagement.ApplicationLayer;
using OrderManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer.MediatR
{
    public class UpdateOrderStatusById
    {
        public class Command : IRequest<Unit>
        {
            public Guid Id { get; set; }
            public string Status { get; set; }
            public Command(Guid orderId, string status)
            {
                Id = orderId;
                Status = status;
            }


        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly OrderService _orderService;
            public Handler(OrderService orderService)
            {
                _orderService = orderService;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _orderService.UpdateOrderStatusById(request.Id, request.Status);
                return Unit.Value;
            }

        }
    }
}







/*
private readonly OrderService _orderService;
private readonly TimeSpan _updateInterval = TimeSpan.FromHours(24);

public Handler(OrderService orderService)
{
    _orderService = orderService;
}
public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
{
    var order = await _orderService.GetOrderByIdAsync(request.Id);

    // functionality that updates order status for evry 24hrs
    if (order != null)
    {
        // Calculate the time elapsed since order creation
        //var timeElapsed = DateTime.UtcNow - order.Date;
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
            await _orderService.UpdateOrderStatusById(request.Id);
        }
    }
    return Unit.Value;
}
*/