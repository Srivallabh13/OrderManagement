using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.ApplicationLayer.MediatR;
using OrderManagement.ApplicationLayer.UserMediatR;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            this._mediator = mediator;
        }
        [HttpGet]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _mediator.Send(new GetAllUsers.Query());
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<User> GetById(string id) 
        {
            return await _mediator.Send(new GetUserById.Query(id));
        }

        [HttpPost]
        [Route("create")]
        public async Task<User> Create(User user) 
        {
            return await _mediator.Send(new CreateUser.Command(user));
        }
        [HttpPut]
        [Route("update")]
        public async Task<Unit> Update(User user)
        {
            return await _mediator.Send(new UpdateUser.Command(user));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task Delete(string id)
        {
            await _mediator.Send(new DeleteUser.Command(id));
        }

    }
}
