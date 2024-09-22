using MediatR;
using Skud.Application.Interfaces;
using Skud.Domain.Entities;

namespace Skud.Application.UseCases.Commands.Doors;
public class CreateDoorCommand : IRequest<int>
{
    public string Location { get; set; }
}
public class CreateDoorCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateDoorCommand, int>
{
    public async Task<int> Handle(CreateDoorCommand request, CancellationToken cancellationToken)
    {
        var addedDoor = await dbContext.Doors.AddAsync(new Door
        {
            Location = request.Location,
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return addedDoor.Entity.Id;
    }
}
