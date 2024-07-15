using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderManagement.ApplicationLayer.MediatR;
using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            try
            {
                return Ok(await _mediator.Send(new GetAllOrders.Query()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all orders.");
                return StatusCode(500, new { message = "An error occurred while fetching all orders." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            try
            {
                var order = await _mediator.Send(new GetOrderById.Query(id));
                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the order with ID: {id}.");
                return StatusCode(500, new { message = "An error occurred while fetching the order" });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetByUser(string userId)
        {
            try
            {
                return Ok(await _mediator.Send(new GetOrdersByUser.Query(userId)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching orders for user ID: {userId}.");
                return StatusCode(500, new { message = "Invalid user id" });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<Order>> Create(OrderDTO order)
        {
            try
            {
                var createdOrder = await _mediator.Send(new CreateOrder.Command(order));
                return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the order.");
                return StatusCode(500, new { message = "An error occurred while creating the order." });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteOrderById.Command(id));
                return StatusCode(200, new { message = "Order deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the order with ID: {id}.");
                return StatusCode(500, new { message = "An error occurred while deleting the order" });
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<Order>> Update(int id, string status)
        {
            try
            {
                await _mediator.Send(new UpdateOrderStatusById.Command(id, status));
                return await _mediator.Send(new GetOrderById.Query(id));
                //return StatusCode(200, new { message = "Order deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the order with ID: {id}.");
                return StatusCode(500, new { message = "An error occurred while updating the order." });
            }
        }
    }
}
