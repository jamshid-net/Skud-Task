﻿using Microsoft.EntityFrameworkCore;
using Skud.Application.Interfaces;
using Skud.Domain.Entities;
using Skud.Domain.Entities.Auth;

namespace Skud.Application.UseCases.Commands.AccessRecords;
public class AccessRecordCommand : IRequest<bool>
{
    public int CardId { get; set; }
    public int DoorId { get; set; }
    public bool IsEntry { get; set; }
}
public class AccessRecordCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<AccessRecordCommand, bool>
{
    public async Task<bool> Handle(AccessRecordCommand request, CancellationToken cancellationToken)
    {
        var foundCardAndUser = await dbContext.Cards.Include(x => x.User)
                             .FirstOrDefaultAsync(x => x.Id == request.CardId, cancellationToken);

        

        var foundUser = foundCardAndUser?.User;

        if(foundCardAndUser is null)
            ThrowExceptionIf.AccessDenied("Access denied user not found!");

        if(foundUser is null)
            ThrowExceptionIf.AccessDenied("Access denied user not found!");

        var foundDoor = await dbContext.Doors.Include(x => x.AccessLevels)
                                                   .FirstOrDefaultAsync(x => x.Id == request.DoorId, cancellationToken);

        ThrowExceptionIf.NotFound(foundDoor);

        var hasAccess = foundDoor!.AccessLevels.Select(x => x.Id).Contains(foundUser!.AccessLevelId);

        if(!hasAccess)
            ThrowExceptionIf.AccessDenied("Access denied, doest found access enter level");


        var newAccessRecord = new AccessRecord
        {
            AccessTime = DateTime.UtcNow,
            DoorId = request.DoorId,
            IsEntry = request.IsEntry,
            UserId = foundUser.Id  
        };
        await dbContext.AccessRecords.AddAsync(newAccessRecord, cancellationToken);

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }
}