using Skud.Application.Interfaces;
using Skud.Application.Models.Auths;
using System.ComponentModel.DataAnnotations;
using Skud.Domain.Entities.Auth;
using Common.Helpers;
using Skud.Domain.Entities;
using AutoMapper;
using Common.Enums;
using Skud.Domain.Enums;

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
        ThrowExceptionIf.ModelIsNull(request);
        var hashSalt = CryptoPassword.CreateHashSalted(request.Password);

        var newUser = new User
        {
            FullName = request.FullName,
            PasswordHash = hashSalt.Hash,
            PasswordSalt = hashSalt.Salt,
            Email = request.Email,
            AccessCard = new Card
            {
                IsActive = true
            },
            AccessLevelId = 3,
            PhoneNumber = request.PhoneNumber,
            RoleId = (int)EnumRole.User,
            Status = EnumUserStatus.Active
        };
        var addedUser = await dbContext.Users.AddAsync(newUser, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<UserResponse>(addedUser.Entity);

    }
}
