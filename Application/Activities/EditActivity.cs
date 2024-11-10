using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities;

public class EditActivity
{
    public class Command(EditActivityDto editActivityDto) : IRequest<Result<Unit>>
    {
        public EditActivityDto EditActivityDto { get; set; } = editActivityDto;
    }

    public class Validator : AbstractValidator<EditActivityDto>
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
        IMapper mapper) : IRequestHandler<Command, Result<Unit>?>
    {
        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities.FindAsync(request.EditActivityDto.Id);

            if (activity is null) return null;

            mapper.Map(request.EditActivityDto, activity);

            var result = await context.SaveChangesAsync() > 0;

            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Failed to update an activity.");
        }
    }
}
