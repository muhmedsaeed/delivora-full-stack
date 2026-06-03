using Delivora.Models;
using Delivora.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Delivora.Services.Service;

public class TokenService : ITokenService
{
    private readonly IConfiguration config;

    private readonly UserManager<AppUser> userManager;


    public TokenService(IConfiguration config, UserManager<AppUser> userManager)
    {
        this.config = config;
        this.userManager = userManager;
    }



    public async Task<(string token, DateTime expiresAt)> GenerateTokenAsync(AppUser user)
    {
        var roles = await userManager.GetRolesAsync(user);


        #region User Claims
        List<Claim> userClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject claim - typically the user ID
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT ID - unique identifier for the token. useful for logout and token revocation scenarios (storing the JTI in a blacklist tokens).

            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("fullName", user.FullName)
        };
        // Add role claims
        userClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        #endregion


        #region Secret Key
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        #endregion

        var expires = DateTime.UtcNow.AddMinutes(
            double.Parse(config["Jwt:ExpiresInMinutes"]!));

        #region Generate Token
        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: userClaims,
            expires: expires,
            signingCredentials: creds
        );
        #endregion

        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}