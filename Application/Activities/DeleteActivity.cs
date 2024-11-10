using Application.Core;
using MediatR;
using Persistence;

namespace Application.Activities;

public class DeleteActivity
{
    public class Command(Guid id) : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; } = id;
    }

    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>?>
    {
        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities.FindAsync(request.Id);

            if (activity is null) return null;

            context.Activities.Remove(activity);

            var result = await context.SaveChangesAsync() > 0;

            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Failed to delete an activity.");
        }
    }
}
