using Skud.Application.Models.Base;

namespace Skud.Application.Models.Auths;
public class UserResponse : BaseDateResponse
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int RoleId { get; set; }
    public int CardId { get; set; }
    public int AccessLevelId { get; set; }
}
