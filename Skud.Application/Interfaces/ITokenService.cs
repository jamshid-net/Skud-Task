using Skud.Application.Models.Auths;

namespace Skud.Application.Interfaces;
public interface ITokenService
{
    Task<TokenResponse> GenerateTokenAsync(int userId, CancellationToken ct = default);

}
