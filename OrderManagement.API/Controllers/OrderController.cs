using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.ApplicationLayer.MediatR;
using OrderManagement.DomainLayer;

namespace OrderManagement.API.Controllers
{
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
        public async Task<Order> GetById(int id)
        {
            //var query = new GetAllOrders.Query();
            //var orders = await _mediator.Send(query);
            return await _mediator.Send(new GetOrderById.Query(id));
            //return Ok(orders);
        }
        [HttpGet]
        [Route("{userId}")]
        public async Task<IEnumerable<Order>> GetByUser(int userId)
        {
            return await _mediator.Send(new GetordersByUser.Query(userId));
        }

        [HttpPost]
        [Route("create")]
        public async Task<Order> Create(Order order)
        {
            return await _mediator.Send(new CreateOrder.Command(order));
        }

        [HttpDelete]
        [Route ("delete/{id}")]
        public async Task<Unit> Delete(int id)
        {
            return await _mediator.Send(new DeleteOrderById.Command(id));
        }

        [HttpPut]
        [Route ("update/{id}")]
        public async Task<Unit> Update(int id)
        {
            return await _mediator.Send(new UpdateOrderStatusById.command(id));
        }
    }
}
