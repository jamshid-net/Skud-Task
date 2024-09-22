using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skud.Application.Interfaces;
using Skud.Application.Models.Doors;

namespace Skud.Application.UseCases.Commands.Doors;
public class UpdateDoorCommand : IRequest<DoorResponse>
{
    public int DoorId { get; set; }
    public string Location { get; set; }
    public int[]? AccessLevelIds { get; set; }
}
public class UpdateDoorCommandHandler(IApplicationDbContext dbContext,
                                      IMapper mapper) : IRequestHandler<UpdateDoorCommand, DoorResponse>
{

    public async Task<DoorResponse> Handle(UpdateDoorCommand request, CancellationToken cancellationToken)
    {
        var foundDoor = await dbContext.Doors.Include(x => x.AccessLevels)
                                                   .FirstOrDefaultAsync(x => x.Id == request.DoorId, cancellationToken);

        ThrowExceptionIf.NotFound(foundDoor);

        foundDoor!.AccessLevels.Clear();
        if (request.AccessLevelIds is { Length: > 0 })
        {
            var foundAccessLevels = await dbContext.AccessLevels.Where(x => request.AccessLevelIds.Contains(x.Id))
                                                                               .ToListAsync(cancellationToken);

            foundDoor.AccessLevels = foundAccessLevels;
        }

        dbContext.Doors.Update(foundDoor);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<DoorResponse>(foundDoor);  
        
    }
}
