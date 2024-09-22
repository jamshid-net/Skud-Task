using Skud.Application.Models.Base;

namespace Skud.Application.Models.Auths;
public class RoleResponse : BaseDateResponse
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public PermissionResponse[]? Permissions { get; set; }
}
