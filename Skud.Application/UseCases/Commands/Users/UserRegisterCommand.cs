using AutoMapper;
using Common.Enums;
using Common.Helpers;
using Serilog;
using Skud.Application.Interfaces;
using Skud.Application.Models.Auths;
using Skud.Domain.Entities;
using Skud.Domain.Entities.Auth;
using Skud.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Skud.Application.UseCases.Commands.Users;
public class UserRegisterCommand : IRequest<UserResponse>
{
    [Required]
    public string FullName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Phone(ErrorMessage = "Invalid phone number.")]
    [Required]
    public string PhoneNumber { get; set; }
}

public class UserRegisterCommandHandler(IApplicationDbContext dbContext,
                                        IMapper mapper) : IRequestHandler<UserRegisterCommand, UserResponse>
{
    public async Task<UserResponse> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.BeginTransactionAsync(cancellationToken);
        ThrowExceptionIf.ModelIsNull(request);
        var hashSalt = CryptoPassword.CreateHashSalted(request.Password);
        try
        {
            var newUser = new User
            {
                FullName = request.FullName,
                PasswordHash = hashSalt.Hash,
                PasswordSalt = hashSalt.Salt,
                Email = request.Email,
                AccessLevelId = 3,
                PhoneNumber = request.PhoneNumber,
                RoleId = (int)EnumRole.User,
                Status = EnumUserStatus.Active
            };
            var addedUser = await dbContext.Users.AddAsync(newUser, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await dbContext.Cards.AddAsync(new Card
            {
                IsActive = true,
                UserId = addedUser.Entity.Id,

            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return mapper.Map<UserResponse>(addedUser.Entity);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            Log.Error(e.Message);
            throw;

        }

    }
}
