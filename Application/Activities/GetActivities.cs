using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class GetActivities
{
    public class Query : IRequest<Result<List<ActivityDto>>> { }

    public class Handler(DataContext context,
        IMapper mapper) : IRequestHandler<Query, Result<List<ActivityDto>>>
    {
        public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken
            cancellationToken)
        {
            var activities = await context.Activities.ToListAsync();

            return Result<List<ActivityDto>>.Success(mapper.Map<List<ActivityDto>>(activities));
        }
    }
}
