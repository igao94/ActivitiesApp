using MediatR;
using Persistence;

namespace Application.Activities;

public class DeleteActivity
{
    public class Command(Guid id) : IRequest
    {
        public Guid Id { get; set; } = id;
    }

    public class Handler(DataContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await context.Activities.FindAsync(request.Id);

            context.Activities.Remove(activity);

            await context.SaveChangesAsync();
        }
    }
}
