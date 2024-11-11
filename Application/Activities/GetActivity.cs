using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class GetActivity
{
    public class Query(Guid id) : IRequest<Result<ActivityDto>>
    {
        public Guid Id { get; set; } = id;
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor,
        IMapper mapper) : IRequestHandler<Query, Result<ActivityDto>?>
    {
        public async Task<Result<ActivityDto>?> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var activity = await context.Activities
                .ProjectTo<ActivityDto>(mapper.ConfigurationProvider,
                    new { currentUsername = userAccesor.GetUsername() })
                .FirstOrDefaultAsync(u => u.Id == request.Id);

            if (activity is null) return null;

            return Result<ActivityDto>.Success(activity);
        }
    }
}
