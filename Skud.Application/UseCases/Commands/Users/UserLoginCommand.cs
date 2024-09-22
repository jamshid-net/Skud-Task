using Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skud.Application.Interfaces;
using Skud.Application.Models.Auths;

namespace Skud.Application.UseCases.Commands.Users;
public class UserLoginCommand : IRequest<TokenResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserLoginCommandHandler(IApplicationDbContext dbContext,
                                     ITokenService tokenService) : IRequestHandler<UserLoginCommand, TokenResponse>
{
    public async Task<TokenResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
    {
        var foundUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower(), cancellationToken);
        ThrowExceptionIf.NotFound(foundUser);

        var hashPassword = CryptoPassword.GetHashSalted(request.Password, foundUser!.PasswordSalt);
        if (foundUser.PasswordHash != hashPassword)
            throw new ErrorFromClientException("Wrong password!");

        var token = await tokenService.GenerateTokenAsync(foundUser.Id, cancellationToken);

        return token;
    }
}
