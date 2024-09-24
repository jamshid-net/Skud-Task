using Microsoft.EntityFrameworkCore;
using Skud.Application.Interfaces;

namespace Skud.Application.UseCases.Commands.AccessLevels;
public class SetAccessLevelToUserCommand : IRequest<bool>
{
    public int UserId { get; set; }
    public int AccessLevelId { get; set; }
}
public class SetAccessLevelToUserCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<SetAccessLevelToUserCommand, bool>
{

    public async Task<bool> Handle(SetAccessLevelToUserCommand request, CancellationToken cancellationToken)
    {
        var foundAccessLevel = await dbContext.AccessLevels.Include(x => x.Users)
                                                                      .FirstOrDefaultAsync(x => x.Id == request.AccessLevelId, cancellationToken);

        ThrowExceptionIf.NotFound(foundAccessLevel);

        var foundUser = await dbContext.Users.FindAsync([request.AccessLevelId], cancellationToken);
        ThrowExceptionIf.NotFound(foundUser);

        foundAccessLevel!.Users.Add(foundUser!);

        dbContext.AccessLevels.Update(foundAccessLevel);

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
