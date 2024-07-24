using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderManagement.ApplicationLayer.MediatR;
using OrderManagement.ApplicationLayer.UserMediatR;
using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            try
            {
                return Ok(await _mediator.Send(new GetAllUsers.Query()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all users.");
                return StatusCode(500, new { message = "An error occurred while fetching all users." });
            }
        }
        //[Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(string id)
        {   
            try
            {
                var user = await _mediator.Send(new GetUserById.Query(id));
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return NotFound(new { message = "User not found." });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the user with ID: {id}.");
                return StatusCode(500, new { message = $"An error occurred while fetching the user,{ex.Message}" });
            }
        }
        //[Authorize]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<User>> Update(string id, UpdateUserDTO user)
        {
            try
            {
                var updatedUser = await _mediator.Send(new UpdateUser.Command(id, user));
                if (updatedUser == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return NotFound(new { message = "User not found." });
                }
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the user with ID: {id}.");
                return StatusCode(500, new { message = $"An error occurred while updating the user, {ex.Message}" });
            }
        }
        //[Authorize]

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _mediator.Send(new DeleteUser.Command(id));
                return StatusCode(200, new { message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the user with ID: {id}.");
                return StatusCode(500, new { message = $"An error occurred while deleting the user,{ex.Message}" });
            }
        }

        [HttpPut("update/role/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<Unit> UpdateRole(string id, string role)
        {
            var result = await _mediator.Send(new UpdateRole.Command(id, role));
            return result;
        }
    }
}
