namespace Skud.Application.Interfaces;
public interface ICurrentUser
{
    public int UserId { get; }
    public int RoleId { get; }
}
