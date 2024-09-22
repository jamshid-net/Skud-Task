using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skud.Application.Interfaces;
using Skud.Application.Models.Doors;

namespace Skud.Application.UseCases.Queries.Doors;
public record GetDoorDetailsQuery(int DoorId) : IRequest<DoorResponse>;

public class GetDoorDetailsQueryHandler(IApplicationDbContext dbContext,
                                        IMapper mapper) : IRequestHandler<GetDoorDetailsQuery, DoorResponse>
{
    public async Task<DoorResponse> Handle(GetDoorDetailsQuery request, CancellationToken cancellationToken)
    {
        var foundDoor = await dbContext.Doors.Include(x => x.AccessLevels)
                                             .FirstOrDefaultAsync(x => x.Id == request.DoorId, cancellationToken);
        
        ThrowExceptionIf.NotFound(foundDoor);

        return mapper.Map<DoorResponse>(foundDoor); 
    }
}
