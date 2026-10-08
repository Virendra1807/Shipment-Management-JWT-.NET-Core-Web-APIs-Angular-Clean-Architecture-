using Application.DTOs.Auth;
using Application.DTOs.Users;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UsersController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserRequestDto request)
        {
            var result = await _userService.CreateUserAsync(request);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Email already exists."
                });
            }

            return Ok(new
            {
                message = "User created successfully."
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            int id,
            UpdateUserRequestDto request)
        {
            var result = await _userService
                .UpdateUserAsync(id, request);

            if (!result)
                return NotFound();

            return Ok(new
            {
                message = "User updated successfully."
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result)
                return NotFound();

            return Ok(new
            {
                message = "User deleted successfully."
            });
        }
        

        [HttpGet("readjwtclaims")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var name = User.FindFirstValue(ClaimTypes.Name);

            var email = User.FindFirstValue(ClaimTypes.Email);

            var role = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new
            {
                userId,
                name,
                email,
                role
            });

        }
    }
}
