using Skud.Application.Models.AccessLevels;
using Skud.Application.Models.Base;

namespace Skud.Application.Models.Doors;
public class DoorResponse : BaseDateResponse
{
    public int Id { get; set; }
    public string Location { get; set; }
    public List<AccessLevelResponse>? AccessLevels { get; set; }
}
