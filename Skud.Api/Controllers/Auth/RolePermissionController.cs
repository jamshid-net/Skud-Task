using Common.Configure;
using Common.Helpers.ZorroTableFilter;
using Common.Repository.Pagination;
using Common.ResponseResult;
using Microsoft.AspNetCore.Mvc;
using Skud.Application.Interfaces;
using Skud.Application.Models.Auths;

namespace Skud.Api.Controllers.Auth;

public class RolePermissionController(IRoleAndPermissionService service) : BaseController
{
    [HttpPost]
    public async Task<ResponseData<RoleResponse>> CreateRole(RoleRequest roleRequestModel, CancellationToken ct)
        => await service.CreateRoleAsync(roleRequestModel, ct);

    [HttpPut]
    public async Task<ResponseData<RoleResponse>> UpdateRole(RoleRequest roleRequestModel, CancellationToken ct)
        => await service.UpdateRoleAsync(roleRequestModel, ct);

    [HttpPost]
    public async Task<ResponseData<PageList<RoleResponse>>> GetAllRoles(ZorroFilterRequest zorroFilterRequest, CancellationToken ct)
        => await service.GetAllRolesAsync(zorroFilterRequest, ct);

    [HttpDelete]
    public async Task<ResponseData<ResponseSuccess>> DeleteRole(int id, CancellationToken ct)
        => await service.DeleteRoleAsync(id, ct);

    [HttpPost]
    public async Task<ResponseData<ResponseSuccess>> SetRoleToUser(UserAndRoleIdRequestModel userAndRole, CancellationToken ct)
        => await service.SetRoleToUserAsync(userAndRole, ct);


    [HttpGet]
    public async Task<ResponseData<RoleResponse>> GetRoleById(int id, CancellationToken ct)
        => await service.GetRoleByIdAsync(id, ct);

    [HttpGet]
    public async Task<PermissionResponse[]> GetUserPermissions(int userId, CancellationToken ct)
        => await service.GetUserPermissionsAsync(userId, ct);

    [HttpGet]
    public PermissionResponse[] GetAllPermissions()
        => service.GetAllPermissions();

}
