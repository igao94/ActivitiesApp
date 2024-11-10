using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Activities;

public class GetActivity
{
    public class Query(Guid id) : IRequest<Result<ActivityDto>>
    {
        public Guid Id { get; set; } = id;
    }

    public class Handler(DataContext context,
        IMapper mapper) : IRequestHandler<Query, Result<ActivityDto>?>
    {
        public async Task<Result<ActivityDto>?> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var activitiy = await context.Activities.FindAsync(request.Id);

            if (activitiy is null) return null;

            return Result<ActivityDto>.Success(mapper.Map<ActivityDto>(activitiy));
        }
    }
}
