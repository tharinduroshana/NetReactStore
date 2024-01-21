using NetStoreAPI.Entities;

namespace NetStoreAPI.Services.Tokens;

public interface ITokenService
{
    string GenerateToken(User user);
}