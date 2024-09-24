using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Helpers.ZorroTableFilter;
using Common.Repository.Pagination;
using Microsoft.EntityFrameworkCore;
using Skud.Application.Interfaces;
using Skud.Application.Models.Auths;

namespace Skud.Application.UseCases.Queries.Users;
public  record GetAllUsersCommand(ZorroFilterRequest ZorroFilterRequest): IRequest<PageList<UserDetailsResponse>>;
public class GetAllUsersCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<GetAllUsersCommand, PageList<UserDetailsResponse>>
{
    public Task<PageList<UserDetailsResponse>> Handle(GetAllUsersCommand request, CancellationToken cancellationToken)
    {

       return dbContext.Users.Include(x => x.Role) 
                              .Include(x => x.AccessLevel)
                              .ThenInclude(x => x.Doors)
                              .Join(dbContext.Cards, user => user.Id, card => card.UserId, 
                              (user, card) => new UserDetailsResponse
                              {
                                  Id = user.Id,
                                  AccessDoorsLocations = user.AccessLevel.Doors.Select(x => x.Location).ToArray(),
                                  AccessLevelName = user.AccessLevel.LevelName,
                                  CardId = card.Id,
                                  CreatedDate = user.CreatedDate,
                                  UpdatedDate = user.UpdatedDate,
                                  Email = user.Email,
                                  FullName = user.FullName,
                                  PhoneNumber = user.PhoneNumber,
                                  RoleName = user.Role.Name,
                              }
                              )
                              .AsNoTracking()
                              .ToPageZorroAsync(request.ZorroFilterRequest, cancellationToken);
    }
}

