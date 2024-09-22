using Common.Configure;
using Common.ResponseResult;
using Skud.Application.UseCases.Commands.AccessRecords;

namespace Skud.Api.Controllers.AccessRecords;

public class AccessRecordController : BaseController
{
    public async Task<ResponseData<ResponseSuccess>> AccessDoor(AccessRecordCommand command, CancellationToken ct)
    => await Mediator.Send(command, ct);

}
