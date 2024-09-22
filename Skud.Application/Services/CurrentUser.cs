using System.Security.Claims;
using Common.JwtAuth;
using Microsoft.AspNetCore.Http;
using Skud.Application.Interfaces;

namespace Skud.Application.Services;
public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public int UserId => Convert.ToInt32(httpContextAccessor.HttpContext?.User?.FindFirstValue(StaticClaims.UserId));

    public int RoleId => Convert.ToInt32(httpContextAccessor.HttpContext?.User?.FindFirstValue(StaticClaims.RoleId));
}