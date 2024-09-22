using Common.JwtAuth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Configure;

[Route("api/[controller]/[action]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();
    private protected int UserId => Convert.ToInt32(User.FindFirst(StaticClaims.UserId)?.Value);
}
