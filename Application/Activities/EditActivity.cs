using Application.Activities.DTOs;
using AutoMapper;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities;

public class EditActivity
{
    public class Command(EditActivityDto editActivityDto) : IRequest
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
        IMapper mapper) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities.FindAsync(request.EditActivityDto.Id);

            mapper.Map(request.EditActivityDto, activity);

            await context.SaveChangesAsync();
        }
    }
}
