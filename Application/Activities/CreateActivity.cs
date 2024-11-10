using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class CreateActivity
{
    public class Command(CreateActivityDto createActivityDto) : IRequest<Result<ActivityDto>>
    {
        public CreateActivityDto CreateActivityDto { get; set; } = createActivityDto;
    }

    public class Validator : AbstractValidator<CreateActivityDto>
    {
        public Validator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Venue).NotEmpty();
        }
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor,
        IMapper mapper) : IRequestHandler<Command, Result<ActivityDto>?>
    {
        public async Task<Result<ActivityDto>?> Handle(Command request,
            CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.UserName == userAccesor.GetUsername());

            if (user is null) return null;

            var activity = mapper.Map<Activity>(request.CreateActivityDto);

            var attendee = new ActivityAttendee
            {
                AppUser = user,
                AppUserId = user.Id,
                Activity = activity,
                ActivityId = activity.Id,
                IsHost = true
            };

            activity.Attendees.Add(attendee);

            context.Activities.Add(activity);

            var result = await context.SaveChangesAsync() > 0;

            return result
                ? Result<ActivityDto>.Success(mapper.Map<ActivityDto>(activity))
                : Result<ActivityDto>.Failure("Failed to create an activity.");
        }
    }
}
