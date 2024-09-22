using Skud.Application.Models.Base;

namespace Skud.Application.Models.AccessLevels;
public class AccessLevelResponse : BaseResponse
{
    public int Id { get; set; }
    public string LevelName { get; set; }
}
