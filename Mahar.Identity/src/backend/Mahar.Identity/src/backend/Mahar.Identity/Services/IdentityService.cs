using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Mahar.Common.DTOs;
using Mahar.Core.Models;

namespace Mahar.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public IdentityService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<UserDto> RegisterAsync(string email, string password, string firstName, string lastName)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return new UserDto
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
            }

            throw new Exception("User registration failed: " + string.Join(", ", result.Errors));
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new Exception("Invalid login attempt.");
            }

            return await _tokenService.CreateToken(user);
        }

        public async Task<string> RefreshTokenAsync(string token)
        {
            // Implementation for refreshing the token
            throw new NotImplementedException();
        }
    }
}