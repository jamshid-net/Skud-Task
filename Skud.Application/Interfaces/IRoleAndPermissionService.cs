using Common.Helpers.ZorroTableFilter;
using Common.Repository.Pagination;
using Skud.Application.Models.Auths;

namespace Skud.Application.Interfaces;
public interface IRoleAndPermissionService
{
    Task<PermissionResponse[]> GetUserPermissionsAsync(int userId, CancellationToken ct = default);
    Task<RoleResponse> CreateRoleAsync(RoleRequest roleRequestModel, CancellationToken ct = default);
    Task<bool> DeleteRoleAsync(int id, CancellationToken ct = default);
    Task<RoleResponse> UpdateRoleAsync(RoleRequest roleRequestModel, CancellationToken ct = default);
    Task<PageList<RoleResponse>> GetAllRolesAsync(ZorroFilterRequest zorroFilterRequest, CancellationToken ct = default);
    Task<bool> SetRoleToUserAsync(UserAndRoleIdRequestModel userIdAndRoleId, CancellationToken ct = default);
    PermissionResponse[] GetAllPermissions();
    Task<RoleResponse> GetRoleByIdAsync(int id, CancellationToken ct = default);
}
