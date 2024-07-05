using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.ApplicationLayer.MediatR;
using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.API.Controllers
{
    [AllowAnonymous]

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpGet]

        public async Task<IEnumerable<Order>> GetAll()
        {
            //var query = new GetAllOrders.Query();
            //var orders = await _mediator.Send(query);
            return await _mediator.Send(new GetAllOrders.Query());
            //return Ok(orders);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Order> GetById(Guid id)
        {
            //var query = new GetAllOrders.Query();
            //var orders = await _mediator.Send(query);
            return await _mediator.Send(new GetOrderById.Query(id));
            //return Ok(orders);
        }
        
        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IEnumerable<Order>> GetByUser(string userId)
        {
            return await _mediator.Send(new GetOrdersByUser.Query(userId));
        }

        [HttpPost]
        [Route("create")]
        public async Task<Order> Create(OrderDTO order)
        {
            return await _mediator.Send(new CreateOrder.Command(order));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<Unit> Delete(Guid id)
        {
            return await _mediator.Send(new DeleteOrderById.Command(id));
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<Unit> Update(Guid id, string status)
        {
            return await _mediator.Send(new UpdateOrderStatusById.Command(id, status));
        }
    }
}
