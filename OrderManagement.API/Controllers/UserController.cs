using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.ApplicationLayer.MediatR;
using OrderManagement.ApplicationLayer.UserMediatR;
using OrderManagement.DomainLayer.DTO;
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

        [HttpPut]
        [Route("update/{id}")]
        public async Task<User> Update(string id, UpdateUserDTO user)
        {
            return await _mediator.Send(new UpdateUser.Command(id, user));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task Delete(string id)
        {
            await _mediator.Send(new DeleteUser.Command(id));
        }


        [HttpPut("{id}/password")]
        public async Task<bool> UpdatePassword(string id, UpdatePasswordDTO model)
        {
            return await _mediator.Send(new UpdatePassword.Command(id, model));
        }
        
        [HttpPut("update/role/{id}")]
        public async Task<Unit> UpdateRole(string id, string role)
        {
            var result = await _mediator.Send(new UpdateRole.Command(id, role));
            return result;
        }
    }
}
