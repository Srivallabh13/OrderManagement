using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.API.Services;
using OrderManagement.DomainLayer.DTO;
using OrderManagement.DomainLayer.Entities;
using System.Security.Claims;

namespace OrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, TokenService tokenService, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            this._tokenService = tokenService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            _logger.LogInformation($"Attempting to log in user with email: {loginDTO.Email}");

            try
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null)
                {
                    _logger.LogWarning($"User with email {loginDTO.Email} not found.");
                    return Unauthorized(new { message = "Invalid email or password." });
                }

                var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
                if (result)
                {
                    _logger.LogInformation($"User {loginDTO.Email} logged in successfully.");
                    return CreateUserObject(user);
                }

                _logger.LogWarning($"Invalid password for user {loginDTO.Email}.");
                return Unauthorized(new { message = "Invalid email or password." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while trying to log in user {loginDTO.Email}.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            try
            {
                if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
                {
                    return BadRequest(new { message = "Username is already taken." });
                }
                if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
                {
                    return BadRequest(new { message = "Email is already taken." });
                }

                var user = new User
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    UserName = registerDto.Username,
                };
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    return CreateUserObject(user);
                }
                return BadRequest(new { message = "User registration failed.", errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to register a new user.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
                if (user == null)
                {
                    _logger.LogWarning($"User with email {User.FindFirstValue(ClaimTypes.Email)} not found.");
                    return NotFound(new { message = "User not found." });
                }
                return CreateUserObject(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to get the current user.");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }
        }

        private UserDTO CreateUserObject(User user)
        {
            return new UserDTO
            {
                Id=user.Id,
                FullName = user.FullName,
                ImageUrl = null,
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
