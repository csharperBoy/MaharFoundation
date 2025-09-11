using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mahar.Identity.DTOs;
using Mahar.Identity.Services;

namespace Mahar.Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var result = await _identityService.RegisterAsync(userDto);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            var result = await _identityService.LoginAsync(userDto);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return Unauthorized(result.Errors);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string token)
        {
            var result = await _identityService.RefreshTokenAsync(token);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return Unauthorized(result.Errors);
        }
    }
}