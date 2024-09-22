using AutoMapper;
using Skud.Application.Interfaces;
using Skud.Application.Models.AccessLevels;
using Skud.Domain.Entities;

namespace Skud.Application.UseCases.Commands.AccessLevels;
public class CreateAccessLevelCommand : IRequest<AccessLevelResponse>
{
    public string LevelName { get; set; }
}
public class CreateAccessLevelCommandHandler(IApplicationDbContext dbContext,
                                             IMapper mapper) : IRequestHandler<CreateAccessLevelCommand, AccessLevelResponse>
{
    public async Task<AccessLevelResponse> Handle(CreateAccessLevelCommand request, CancellationToken cancellationToken)
    {
        var newAccessLevel = new AccessLevel
        {
            LevelName = request.LevelName,
        };
        var addedAccessLevel =  (await dbContext.AccessLevels.AddAsync(newAccessLevel, cancellationToken)).Entity;

        return mapper.Map<AccessLevelResponse>(addedAccessLevel);
    }
}
