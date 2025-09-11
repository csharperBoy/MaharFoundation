namespace Mahar.Identity.Services
{
    public interface IIdentityService
    {
        Task<UserDto> RegisterAsync(UserDto userDto);
        Task<UserDto> LoginAsync(string username, string password);
        Task<string> RefreshTokenAsync(string token);
    }
}