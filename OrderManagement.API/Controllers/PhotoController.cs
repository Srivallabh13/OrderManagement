using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.ApplicationLayer.Photos.PhotoMediatR;
using OrderManagement.DomainLayer.Entities;

namespace OrderManagement.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IMediator mediator;

        public PhotoController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        public async Task<Photo> Add([FromForm] Add.Command command)
        {
            return await mediator.Send(command);
        }
        [HttpDelete]
        [Route("/delete/{id}")]
        public async Task<Unit> Delete(string id)
        {
            return await mediator.Send(new DeletePhoto.Command { Id = id });
        }
    }
}
