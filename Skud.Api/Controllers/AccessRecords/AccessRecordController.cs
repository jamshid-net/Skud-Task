using Common.Configure;
using Common.Helpers.ZorroTableFilter;
using Common.ResponseResult;
using Microsoft.AspNetCore.Mvc;
using Skud.Application.UseCases.Commands.AccessRecords;
using Skud.Application.UseCases.Queries.AccessRecords;

namespace Skud.Api.Controllers.AccessRecords;

public class AccessRecordController : BaseController
{
    [HttpPost]
    public async Task<ResponseData<ResponseSuccess>> AccessDoor(AccessRecordCommand command, CancellationToken ct)
    => await Mediator.Send(command, ct);


    [HttpPost]
    public async Task<FileResult> GetAccessRecordExcel(ZorroFilterRequest zorroFilterRequest, CancellationToken ct)
    {
        var fileRecord = await Mediator.Send(new GetAllAccessRecordsExcelQuery(zorroFilterRequest), ct);
        return File(fileRecord.FileBytes, fileRecord.ContentType, fileRecord.FileName);
    }

}
