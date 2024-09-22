namespace Skud.Application.Models.Auths;
public class RoleRequest
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int[]? PermissionIds { get; set; }
}
