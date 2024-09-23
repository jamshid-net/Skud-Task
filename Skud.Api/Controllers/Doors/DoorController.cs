using Common.Configure;
using Common.Helpers.ZorroTableFilter;
using Common.Repository.Pagination;
using Common.ResponseResult;
using Microsoft.AspNetCore.Mvc;
using Skud.Application.Models.Doors;
using Skud.Application.UseCases.Commands.Doors;
using Skud.Application.UseCases.Queries.Doors;

namespace Skud.Api.Controllers.Doors;
public class DoorController : BaseController
{
    [HttpPost]
    public async Task<ResponseData<int>> CreateDoor(CreateDoorCommand command, CancellationToken ct)
    => await Mediator.Send(command, ct);

    [HttpPut]
    public async Task<ResponseData<DoorResponse>> UpdateDoor(UpdateDoorCommand command, CancellationToken ct)
    => await Mediator.Send(command, ct);

    [HttpPost]
    public async Task<PageList<DoorResponse>> GetAllDoors(ZorroFilterRequest zorroFilterRequest, CancellationToken ct)
    => await Mediator.Send(new GetAllDoorsQuery(zorroFilterRequest), ct);
}
