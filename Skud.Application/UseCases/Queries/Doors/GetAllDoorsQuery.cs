using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Helpers.ZorroTableFilter;
using Common.Repository.Pagination;
using Microsoft.EntityFrameworkCore;
using Skud.Application.Interfaces;
using Skud.Application.Models.Doors;

namespace Skud.Application.UseCases.Queries.Doors;
public record GetAllDoorsQuery(ZorroFilterRequest ZorroFilterRequest) : IRequest<PageList<DoorResponse>>;

public class GetAllDoorsQueryHandler(IApplicationDbContext dbContext,
                                     IMapper mapper) : IRequestHandler<GetAllDoorsQuery, PageList<DoorResponse>>
{
    public async Task<PageList<DoorResponse>> Handle(GetAllDoorsQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Doors.IgnoreAutoIncludes()
                                                         .AsNoTracking()
                                                         .ProjectTo<DoorResponse>(mapper.ConfigurationProvider);

        var result = await query.ToPageZorroAsync(request.ZorroFilterRequest, cancellationToken);

        return result;

    }
}
