using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NetStoreAPI.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace NetStoreAPI.Services.Tokens;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var claims = new ClaimsIdentity(new[]
        {
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.GivenName, user.Username),
        });
        var issuer = _configuration.GetSection("AppSettings:Issuer").Value;
        var audience = _configuration.GetSection("AppSettings:Audience").Value;
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)),
            SecurityAlgorithms.HmacSha512Signature
        );
        var expires = DateTime.Now.AddDays(30);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = signingCredentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return jwtToken;
    }
}