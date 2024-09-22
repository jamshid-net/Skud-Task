using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Helpers.ZorroTableFilter;
using Common.Repository.Pagination;
using Microsoft.EntityFrameworkCore;
using Skud.Application.Interfaces;
using Skud.Application.Models.AccessLevels;

namespace Skud.Application.UseCases.Queries.AccessLevels;
public record GetAllAccessLevelsQuery(ZorroFilterRequest ZorroFilterRequest) : IRequest<PageList<AccessLevelResponse>>;

public class GetAllAccessLevelsQueryHandler(IApplicationDbContext dbContext,
                                            IMapper mapper) : IRequestHandler<GetAllAccessLevelsQuery, PageList<AccessLevelResponse>>
{
    public Task<PageList<AccessLevelResponse>> Handle(GetAllAccessLevelsQuery request, CancellationToken cancellationToken)
    {
        return dbContext.AccessLevels.IgnoreAutoIncludes()
                                     .ProjectTo<AccessLevelResponse>(mapper.ConfigurationProvider)
                                     .ToPageZorroAsync(request.ZorroFilterRequest, cancellationToken);
    }
}
