using Application.Core;
using Application.Interfaces;
using Application.Profiles.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers;

public class GetUsersFollowing
{
    public class Query : IRequest<Result<List<ProfileDto>>>
    {
        public required string Predicate { get; set; }
        public required string UserName { get; set; }
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor,
        IMapper mapper) : IRequestHandler<Query, Result<List<ProfileDto>>>
    {
        public async Task<Result<List<ProfileDto>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            List<ProfileDto> profiles = [];

            switch (request.Predicate)
            {
                case "followers":
                    profiles = await context.UsersFollowings
                        .Where(u => u.Target.UserName == request.UserName)
                        .Select(o => o.Observer)
                        .ProjectTo<ProfileDto>(mapper.ConfigurationProvider,
                            new { currentUsername = userAccesor.GetUsername() })
                        .ToListAsync();
                    break;

                case "following":
                    profiles = await context.UsersFollowings
                        .Where(u => u.Observer.UserName == request.UserName)
                        .Select(t => t.Target)
                        .ProjectTo<ProfileDto>(mapper.ConfigurationProvider,
                            new { currentUsername = userAccesor.GetUsername() })
                        .ToListAsync();
                    break;
            }

            return Result<List<ProfileDto>>.Success(profiles);
        }
    }
}
