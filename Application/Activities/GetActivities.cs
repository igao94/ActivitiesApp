using Application.Activities.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class GetActivities
{
    public class Query : IRequest<List<ActivityDto>> { }

    public class Handler(DataContext context,
        IMapper mapper) : IRequestHandler<Query, List<ActivityDto>>
    {
        public async Task<List<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activities = await context.Activities.ToListAsync();

            return mapper.Map<List<ActivityDto>>(activities);
        }
    }
}
