using Delivora.Models;

namespace Delivora.Services.IService;

public interface ITokenService
{
    Task<(string token, DateTime expiresAt)> GenerateTokenAsync(AppUser user);
}
