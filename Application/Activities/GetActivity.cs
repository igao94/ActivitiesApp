using Application.Activities.DTOs;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Activities;

public class GetActivity
{
    public class Query(Guid id) : IRequest<ActivityDto>
    {
        public Guid Id { get; set; } = id;
    }

    public class Handler(DataContext context,
        IMapper mapper) : IRequestHandler<Query, ActivityDto?>
    {
        public async Task<ActivityDto?> Handle(Query request, CancellationToken cancellationToken)
        {
            var activitiy = await context.Activities.FindAsync(request.Id);

            if (activitiy is null) return null;

            return mapper.Map<ActivityDto>(activitiy);
        }
    }
}
