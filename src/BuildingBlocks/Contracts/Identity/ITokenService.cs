using Contracts.Models;

namespace Contracts.Identity;

public interface ITokenService
{
    TokenResponse GetToken(TokenRequest request);
}
