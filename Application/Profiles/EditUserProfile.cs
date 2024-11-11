using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class EditUserProfile
{
    public class Command : IRequest<Result<Unit>>
    {
        public required string DisplayName { get; set; }
        public string? Bio { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
        }
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor) : IRequestHandler<Command, Result<Unit>?>
    {
        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.UserName == userAccesor.GetUsername());

            if (user is null) return null;

            user.DisplayName = request.DisplayName ?? user.DisplayName;
            user.Bio = request.Bio ?? user.Bio;

            var result = await context.SaveChangesAsync() > 0;

            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Problem updating profile.");
        }
    }
}
