using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookingApp.Api.Auth;
using BookingApp.Application.Auth;
using BookingApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {


            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                return BadRequest("User with this email already exists.");
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FullName = request.FullName
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(request.Role));
                if (!roleResult.Succeeded)
                {
                    return StatusCode(500, "Failed to create role.");
                }
            }

            await _userManager.AddToRoleAsync(user, request.Role);
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email!, user.FullName ?? string.Empty, roles, out var expiresAt);
            return Ok(new AuthResponse(token, expiresAt));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                return Unauthorized("Invalid credentials.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email!, user.FullName ?? string.Empty, roles, out var expiresAt);

            return Ok(new AuthResponse(token, expiresAt));
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var fullName = User.FindFirstValue("fullName");
            return Ok(new
            {
                userId,
                email,
                fullName,
                roles = User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
            });
        }
    }
}
