using System.Security.Claims;
using Common.Enums;
using Common.ResponseResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.JwtAuth;

public class CustomAuthorizeAttribute : TypeFilterAttribute
{
    public CustomAuthorizeAttribute(params EnumPermission[] enumPermissions)
    : base(typeof(AuthorizeActionFilter))
    {
        Arguments = [enumPermissions];
    }
}

public class AuthorizeActionFilter(params EnumPermission[] enumPermissions) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        var responseStatus = ResponseStatus.Unauthorized;

        
        var permissions = context.HttpContext.User.FindAll(StaticClaims.Permissions)
                                                             .Select(x => x.Value)
                                                             .ToList() ?? [];



        if(permissions is {Count: > 0})
        {
            responseStatus = ResponseStatus.Forbidden;
        }

        #region CommentedCode
        /*
        var authHeader = context.HttpContext.Request.Headers["Authorization"];
        var token = authHeader.FirstOrDefault()?.Split(" ").Last();

        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = AuthOptions.ISSUER,
            ValidAudience = AuthOptions.AUDIENCE
        }, out SecurityToken validateToken);

        var jwtToken = (JwtSecurityToken)validateToken;
        */

        //   hasClaim = true; // vaqtincha formalarni yasab bo`lgandan keyin o`chiriladi
        #endregion
        //var hasClaim = permissions.Any(o => o.StartsWith(permission));
        var enumPermissionStrings = enumPermissions.Select(p => p.ToString()).ToList();
        var hasClaim = permissions.Any(o => enumPermissionStrings.Any(ep => ep.Equals(o, StringComparison.CurrentCultureIgnoreCase)));
        if (!hasClaim)
        {
            context.HttpContext.Response.StatusCode = (int)responseStatus;
            var result = new ResponseData<object>(responseStatus);
            context.Result = new ObjectResult(result);
        }
        else
            await next();

    }
}

