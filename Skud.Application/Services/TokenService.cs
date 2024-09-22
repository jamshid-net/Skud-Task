using Common.Extensions;
using Common.JwtAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Skud.Application.Interfaces;
using Skud.Domain.Entities.Auth;
using Skud.Domain.Enums;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Skud.Application.Models.Auths;

namespace Skud.Application.Services;
public class TokenService(IApplicationDbContext dbContext) : ITokenService
{
    public async Task<TokenResponse> GenerateTokenAsync(int userId, CancellationToken ct = default)
    {

        var user = await dbContext.Users.Include(x => x.Role)
                                              .ThenInclude(x => x.Permissions)
                                              .FirstOrDefaultAsync(x => x.Id == userId, ct);

        if (user == null)
            throw new NotFoundException();

        if (user.Status != EnumUserStatus.Active)
            throw new AccessDeniedException("User is not active!");


        var data = GetJwtAsync(user);


        return data;

    }

    private TokenResponse GetJwtAsync(User user)
    {
        var utcNow = DateTime.UtcNow;

        var permissions = user.Role.Permissions;

        var permissionClaims = permissions?.Select(p => new Claim(StaticClaims.Permissions, p.Name))
                                                     .ToList();

        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToLowerString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture)),
            new Claim(StaticClaims.Email, user.Email.ToLower()),
            new Claim(StaticClaims.UserId, user.Id.ToString()),
            new Claim(StaticClaims.RoleId, user.RoleId.ToString()),
            new Claim(StaticClaims.Status, user.Status.ToString())

        ];

        if (permissionClaims is not null || permissionClaims?.Count > 0)
            claims.AddRange(permissionClaims);


        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: claims,
            notBefore: utcNow,
            expires: utcNow.Add(TimeSpan.FromMinutes(AuthOptions.ExpireMinutes)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        var expTime = (long)TimeSpan.FromMinutes(AuthOptions.ExpireMinutes).TotalSeconds;

        var token = new TokenResponse
        {
            AccessToken = encodedJwt,
            Expires = expTime,
        };

        return token;
    }
}
