using Skud.Application.Interfaces;

namespace Skud.Application.UseCases.Commands.AccessLevels;
public class UpdateAccessLevelCommand : IRequest<bool>
{
    public string Id { get; set; }
    public string AccessLevelName { get; set; }
}
public class UpdateAccessLevelCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateAccessLevelCommand, bool>
{
    public async Task<bool> Handle(UpdateAccessLevelCommand request, CancellationToken cancellationToken)
    {
        var foundAccessLevel = await dbContext.AccessLevels.FindAsync([request.Id], cancellationToken);
        ThrowExceptionIf.NotFound(foundAccessLevel);

        foundAccessLevel!.LevelName = request.AccessLevelName;

        dbContext.AccessLevels.Update(foundAccessLevel);
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
