using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;

namespace Application.Activities;

public class GetActivities
{
    public class Query(ActivityParams activityParams) : IRequest<Result<PagedList<ActivityDto>>>
    {
        public ActivityParams ActivityParams { get; set; } = activityParams;
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor,
        IMapper mapper) : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
    {
        public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken
            cancellationToken)
        {
            var query = context.Activities
                .Where(a => a.Date >= request.ActivityParams.StartDate)
                .OrderBy(a => a.Date)
                .ProjectTo<ActivityDto>(mapper.ConfigurationProvider,
                    new { currentUsername = userAccesor.GetUsername() })
                .AsQueryable();

            if (request.ActivityParams.IsGoing && !request.ActivityParams.IsHost)
            {
                query = query.Where(a => a.Attendees.Any(aa => aa.Username == userAccesor.GetUsername()));
            }

            if (request.ActivityParams.IsHost && !request.ActivityParams.IsGoing)
            {
                query = query.Where(a => a.HostUsername == userAccesor.GetUsername());
            }

            var activities = await PagedList<ActivityDto>
                .CreateAsync(query, request.ActivityParams.PageNumber, request.ActivityParams.PageSize);

            return Result<PagedList<ActivityDto>>.Success(activities);
        }
    }
}
