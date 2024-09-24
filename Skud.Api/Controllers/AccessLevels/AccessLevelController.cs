using Common.Configure;
using Common.Helpers.ZorroTableFilter;
using Common.Repository.Pagination;
using Common.ResponseResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skud.Application.Models.AccessLevels;
using Skud.Application.UseCases.Commands.AccessLevels;
using Skud.Application.UseCases.Queries.AccessLevels;

namespace Skud.Api.Controllers.AccessLevels;

public class AccessLevelController : BaseController
{
    [HttpPost]
    public async Task<ResponseData<AccessLevelResponse>> CreateAccessLevel(CreateAccessLevelCommand command, CancellationToken ct)
    => await Mediator.Send(command, ct);  

    [HttpPut]
    public async Task<ResponseData<ResponseSuccess>> UpdateAccessLevel(UpdateAccessLevelCommand command, CancellationToken ct)
    => await Mediator.Send(command, ct);  

    [HttpPost]
    public async Task<ResponseData<PageList<AccessLevelResponse>>> GetAllAccessLevels(ZorroFilterRequest zorroFilterRequest, CancellationToken ct)
    => await Mediator.Send(new GetAllAccessLevelsQuery(zorroFilterRequest), ct);

    [HttpPut]
    public async Task<ResponseData<ResponseSuccess>> SetAccessLevelToUser(SetAccessLevelToUserCommand command, CancellationToken ct)
    => await Mediator.Send(command, ct);

}
