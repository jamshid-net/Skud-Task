using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Enums;
using Common.Helpers.ZorroTableFilter;
using Common.Repository.Pagination;
using Microsoft.EntityFrameworkCore;
using Skud.Application.Interfaces;
using Skud.Application.Models.Auths;
using Skud.Domain.Entities.Auth;

namespace Skud.Application.Services;
public class RoleAndPermissionService(IApplicationDbContext dbContext,
                                      IMapper mapper) : IRoleAndPermissionService
{
    public async Task<PermissionResponse[]> GetUserPermissionsAsync(int userId, CancellationToken ct = default)
    {
        var foundUser = await dbContext.Users.Include(x => x.Role)
                                                  .ThenInclude(x => x.Permissions)
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(x => x.Id == userId, ct);

        if(foundUser is null)
            throw new NotFoundException($"User with id: {userId} not found!");

        return foundUser.Role.Permissions.Select(x => new PermissionResponse
        {
            Id = x.Id,
            Name = x.Name,
        }).ToArray();                                                
    }

    public async Task<RoleResponse> CreateRoleAsync(RoleRequest roleRequestModel, CancellationToken ct = default)
    {
        if (roleRequestModel is null)
            throw new ModelIsNullException(nameof(RoleRequest));

        if(await dbContext.Roles.AnyAsync(x => x.Name.ToLower() == roleRequestModel.Name!.ToLower(), ct))
        {
            throw new ConflictException($"Role with name: {roleRequestModel.Name} already exist!");
        }

        var permissionIds = roleRequestModel.PermissionIds;

        var newRole = new Role
        {
            Name = roleRequestModel.Name,
        };
        var newAddedRole = (await dbContext.Roles.AddAsync(newRole, ct)).Entity;
        
        if (permissionIds is not null && permissionIds.Length > 0)
        {
            newAddedRole.Permissions = await dbContext.Permissions.Where(x => permissionIds.Contains(x.Id))
                                                                  .ToListAsync(ct);
            
        }
        await dbContext.SaveChangesAsync(ct);
        return mapper.Map<RoleResponse>(newAddedRole);
    }

    public async Task<bool> DeleteRoleAsync(int id, CancellationToken ct = default)
    {
        var foundRole = await dbContext.Roles.FindAsync([id], ct);
        if (foundRole is null)
            throw new NotFoundException($"Role with id : {id} not found!");

        var notDeleteAbleRoles = Enum.GetValues(typeof(EnumRole)).Cast<int>();
        if (notDeleteAbleRoles.Any(x => x == id))
            throw new ErrorFromClientException($"You can not remove this role, role id:{id}!");

        dbContext.Roles.Remove(foundRole);
        return await dbContext.SaveChangesAsync(ct) > 0;
    }


    public async Task<PageList<RoleResponse>> GetAllRolesAsync(ZorroFilterRequest zorroFilterRequest, CancellationToken ct = default)
    {
        var roles = await dbContext.Roles.AsNoTracking()
                                                         .Include(x => x.Permissions)
                                                         .ProjectTo<RoleResponse>(mapper.ConfigurationProvider)
                                                         .ToPageZorroAsync(zorroFilterRequest, ct);
        return roles;
    }

   
    public async Task<RoleResponse> UpdateRoleAsync(RoleRequest roleModel, CancellationToken ct = default)
    {
        if (roleModel?.Id is null or 0)
            throw new ModelIsNullException(nameof(RoleRequest), "Role model or Id is null or zero!");

        var foundRole = await dbContext.Roles.Include(x => x.Permissions)
                                                   .FirstOrDefaultAsync(x => x.Id == roleModel.Id, ct);

        if (foundRole is null)
            throw new NotFoundException($"Role with id : {roleModel.Id} not found!");


        var permissionIds = roleModel.PermissionIds;
        foundRole.Permissions.Clear();

        if (permissionIds is not null && permissionIds.Length > 0)
        {
            var foundPermissions = await dbContext.Permissions.Where(x => permissionIds.Contains(x.Id))
                                                                             .ToListAsync(ct);

            foreach (var permission in foundPermissions)
            {
                foundRole.Permissions.Add(permission);
            }
        }

        
        dbContext.Roles.Update(foundRole);
        await dbContext.SaveChangesAsync(ct);

        return mapper.Map<RoleResponse>(foundRole);
    }

    public async Task<bool> SetRoleToUserAsync(UserAndRoleIdRequestModel userIdAndRoleId, CancellationToken ct = default)
    {
        var foundUser = await dbContext.Users.FindAsync([userIdAndRoleId.UserId], ct);
        if (foundUser is null)
            throw new NotFoundException($"User with id:{userIdAndRoleId.UserId} not found!");

        var roleIsExist = await dbContext.Roles.AnyAsync(x => x.Id == userIdAndRoleId.RoleId, ct);
        if (!roleIsExist)
            throw new NotFoundException($"Role with id:{userIdAndRoleId.RoleId} not found!");

        foundUser.RoleId = userIdAndRoleId.RoleId;
        dbContext.Users.Update(foundUser);
        return await dbContext.SaveChangesAsync(ct) > 0;
    }
    public PermissionResponse[] GetAllPermissions()
    {
        return Enum.GetValues(typeof(EnumPermission))
            .Cast<EnumPermission>()
            .Select(p => new PermissionResponse
            {
                Id = (int)p,
                Name = p.ToString()
            })
            .ToArray();
    }

    public async Task<RoleResponse> GetRoleByIdAsync(int id, CancellationToken ct = default)
    {
        var foundRole = await dbContext.Roles.Include(x => x.Permissions)
                                                   .AsNoTracking()
                                                   .FirstOrDefaultAsync(x => x.Id == id, ct);
       
        if (foundRole is null)
            throw new NotFoundException($"Role with id: {id} not found!");

        return mapper.Map<RoleResponse>(foundRole);
    }
}
