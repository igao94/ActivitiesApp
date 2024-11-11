using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers;

public class GetUsersFollowing
{
    public class Query : IRequest<Result<List<Profiles.DTOs.ProfileDto>>>
    {
        public required string Predicate { get; set; }
        public required string UserName { get; set; }
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor,
        IMapper mapper) : IRequestHandler<Query, Result<List<Profiles.DTOs.ProfileDto>>>
    {
        public async Task<Result<List<Profiles.DTOs.ProfileDto>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            List<Profiles.DTOs.ProfileDto> profiles = [];

            switch (request.Predicate)
            {
                case "followers":
                    profiles = await context.UsersFollowings
                        .Where(u => u.Target.UserName == request.UserName)
                        .Select(o => o.Observer)
                        .ProjectTo<Profiles.DTOs.ProfileDto>(mapper.ConfigurationProvider,
                            new { currentUsername = userAccesor.GetUsername() })
                        .ToListAsync();
                    break;

                case "following":
                    profiles = await context.UsersFollowings
                        .Where(u => u.Observer.UserName == request.UserName)
                        .Select(t => t.Target)
                        .ProjectTo<Profiles.DTOs.ProfileDto>(mapper.ConfigurationProvider,
                            new { currentUsername = userAccesor.GetUsername() })
                        .ToListAsync();
                    break;
            }

            return Result<List<Profiles.DTOs.ProfileDto>>.Success(profiles);
        }
    }
}
