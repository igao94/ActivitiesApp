using Application.Core;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class UpdateAttendance
{
    public class Command(Guid id) : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; } = id;
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor) : IRequestHandler<Command, Result<Unit>?>
    {
        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.UserName == userAccesor.GetUsername());

            if (user is null) return null;

            var activity = await context.Activities
                .Include(a => a.Attendees)
                .ThenInclude(aa => aa.AppUser)
                .FirstOrDefaultAsync(a => a.Id == request.Id);

            if (activity is null) return null;

            var hostUsername = user.Activities.FirstOrDefault(u => u.IsHost)?.AppUser?.UserName;

            var attendance = activity.Attendees
                .FirstOrDefault(aa => aa.AppUser.UserName == user.UserName);

            if (attendance is not null && hostUsername == user.UserName)
                activity.IsCancelled = !activity.IsCancelled;

            if (attendance is not null && hostUsername != user.UserName)
                activity.Attendees.Remove(attendance);

            if (attendance is null)
            {
                attendance = new ActivityAttendee
                {
                    AppUser = user,
                    AppUserId = user.Id,
                    Activity = activity,
                    ActivityId = activity.Id,
                    IsHost = false
                };

                activity.Attendees.Add(attendance);
            }

            var result = await context.SaveChangesAsync() > 0;

            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Problem updating attendance.");
        }
    }
}
