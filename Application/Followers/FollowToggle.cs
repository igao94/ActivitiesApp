using Application.Core;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers;

public class FollowToggle
{
    public class Command(string username) : IRequest<Result<Unit>>
    {
        public string Username { get; set; } = username;
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor) : IRequestHandler<Command, Result<Unit>?>
    {
        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var observer = await context.Users
                .FirstOrDefaultAsync(u => u.UserName == userAccesor.GetUsername());

            if (observer is null) return null;

            var target = await context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.Username);

            if (target is null) return null;

            if (observer == target) return Result<Unit>.Failure("Can't follow yourself.");

            var userFollowing = await context.UsersFollowings.FindAsync(observer.Id, target.Id);

            if (userFollowing is null)
            {
                userFollowing = new UserFollowing
                {
                    ObserverId = observer.Id,
                    Observer = observer,
                    TargetId = target.Id,
                    Target = target
                };

                context.UsersFollowings.Add(userFollowing);
            }
            else
            {
                context.UsersFollowings.Remove(userFollowing);
            }

            var result = await context.SaveChangesAsync() > 0;

            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Failed to follow user.");
        }
    }
}
