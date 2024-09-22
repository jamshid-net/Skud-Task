using Common.Configure;
using Common.ResponseResult;
using Skud.Application.Models.Auths;
using Skud.Application.UseCases.Commands.Users;

namespace Skud.Api.Controllers.Auth;

public class AuthUserController : BaseController
{
    public async Task<ResponseData<TokenResponse>> Login(UserLoginCommand command, CancellationToken cancellationToken)
    => await Mediator.Send(command, cancellationToken);

    public async Task<ResponseData<UserResponse>> Register(UserRegisterCommand command, CancellationToken cancellationToken)
    => await Mediator.Send(command, cancellationToken);
}
