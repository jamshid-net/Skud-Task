using Skud.Application.Models.Base;

namespace Skud.Application.Models.Auths;
public class UserDetailsResponse : BaseDateResponse
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string RoleName { get; set; }
    public int CardId { get; set; }
    public string AccessLevelName { get; set; }
    public string[] AccessDoorsLocations { get; set; }
}
