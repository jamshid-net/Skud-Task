using ClosedXML.Excel;
using Common.Helpers.ZorroTableFilter;
using Common.Models;
using Common.Repository.Pagination;
using Microsoft.EntityFrameworkCore;
using Skud.Application.Interfaces;
using Skud.Application.Models.AccessRecords;
using System.Data;

namespace Skud.Application.UseCases.Queries.AccessRecords;
public record GetAllAccessRecordsExcelQuery(ZorroFilterRequest zorroFilterRequest) : IRequest<FileRecord>;

public class GetAllAccessLevelsExcelQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetAllAccessRecordsExcelQuery, FileRecord>
{
    public async Task<FileRecord> Handle(GetAllAccessRecordsExcelQuery request, CancellationToken cancellationToken)
    {
        using XLWorkbook wb = new XLWorkbook();

        var userData = await GetUserAccessRecordDataTable(request.zorroFilterRequest, cancellationToken);
        var sheet1 = wb.AddWorksheet(userData, "UsersAccessTimeRecord");

        sheet1.Columns().AdjustToContents(20.0, 80.0);
        sheet1.RowHeight = 20;
        using MemoryStream ms = new MemoryStream();
        wb.SaveAs(ms);
        return new FileRecord(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"UsersAccessTimeRecord.xlsx");
    }

    private async Task<DataTable> GetUserAccessRecordDataTable(ZorroFilterRequest request, CancellationToken cancellationToken)
    {
        var result = await dbContext.AccessRecords.Include(x => x.Door)
            .Include(x => x.User)
            .Select(x => new AccessRecordResponseToExcel
            {
                AccessTime = x.AccessTime,
                DoorLocation = x.Door.Location,
                UserFullName = x.User.FullName,
                UserEmail = x.User.Email,
                Entry = x.IsEntry ? "Enter" : "Exit"
            })
            .ToPageZorroAsync(request, cancellationToken);

        DataTable dt = new DataTable();
        dt.TableName = "User skud";
        dt.Columns.Add("FullName", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Door location", typeof(string));
        dt.Columns.Add("Time", typeof(string));
        dt.Columns.Add("Entry", typeof(string));


        var data = result.Items;

        if (data.Count > 0)
        {
            data.ForEach(item =>
            {
                dt.Rows.Add(item.UserFullName, 
                            item.UserEmail, 
                            item.DoorLocation, 
                            item.AccessTime.ToString("dd-MM-yyyy hh:mm:ss"), 
                            item.Entry);

            });
        }

        return dt;
    }

}
