using Common.Configure;
using Common.Helpers.ZorroTableFilter;
using Common.Repository.Pagination;
using Common.ResponseResult;
using Microsoft.AspNetCore.Mvc;
using Skud.Application.Models.Auths;
using Skud.Application.UseCases.Commands.Users;
using Skud.Application.UseCases.Queries.Users;

namespace Skud.Api.Controllers.Auth;

public class AuthUserController : BaseController
{
    [HttpPost]
    public async Task<ResponseData<TokenResponse>> Login(UserLoginCommand command, CancellationToken cancellationToken)
    => await Mediator.Send(command, cancellationToken);

    [HttpPost]
    public async Task<ResponseData<UserResponse>> Register(UserRegisterCommand command, CancellationToken cancellationToken)
    => await Mediator.Send(command, cancellationToken);

    [HttpPost]
    public async Task<ResponseData<PageList<UserDetailsResponse>>> GetAllUsers(ZorroFilterRequest zorroFilterRequest, CancellationToken cancellationToken)
    => await Mediator.Send(new GetAllUsersCommand(zorroFilterRequest), cancellationToken);
}
