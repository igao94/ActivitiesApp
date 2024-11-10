using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
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
        IMapper mapper) : IRequestHandler<Command, Result<ActivityDto>>
    {
        public async Task<Result<ActivityDto>> Handle(Command request,
            CancellationToken cancellationToken)
        {
            var activity = mapper.Map<Activity>(request.CreateActivityDto);

            context.Activities.Add(activity);

            var result = await context.SaveChangesAsync() > 0;

            return result
                ? Result<ActivityDto>.Success(mapper.Map<ActivityDto>(activity))
                : Result<ActivityDto>.Failure("Failed to create an activity.");
        }
    }
}
