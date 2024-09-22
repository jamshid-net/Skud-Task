using AutoMapper;
using Common.Extensions;
using Skud.Application.Models.AccessLevels;
using Skud.Application.Models.Auths;
using Skud.Application.Models.Doors;
using Skud.Domain.Entities;
using Skud.Domain.Entities.Auth;

namespace Skud.Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CommonMapping();

        //AccessLevel 
        CreateMap<AccessLevelResponse, AccessLevel>().ReverseMap();

        //Door 
        CreateMap<DoorResponse, Door>().ReverseMap();

        //Auth
        CreateMap<UserResponse, User>().ReverseMap();
        CreateMap<PermissionResponse, Permission>().ReverseMap();
        CreateMap<RoleResponse, Role>().ReverseMap();
    }
}
