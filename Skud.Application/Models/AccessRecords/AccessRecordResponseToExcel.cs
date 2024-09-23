namespace Skud.Application.Models.AccessRecords;
public class AccessRecordResponseToExcel
{
    public string UserFullName { get; set; }
    public string UserEmail { get; set; }   
    public DateTime AccessTime { get; set; }
    public string DoorLocation { get; set; }
    public string Entry {  get; set; }
}
