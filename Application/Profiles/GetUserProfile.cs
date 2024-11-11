using Application.Core;
using Application.Interfaces;
using Application.Profiles.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class GetUserProfile
{
    public class Query(string username) : IRequest<Result<ProfileDto>>
    {
        public string Username { get; set; } = username;
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor,
        IMapper mapper) : IRequestHandler<Query, Result<ProfileDto>?>
    {
        public async Task<Result<ProfileDto>?> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .ProjectTo<ProfileDto>(mapper.ConfigurationProvider,
                    new { currentUsername = userAccesor.GetUsername() })
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user is null) return null;

            return Result<ProfileDto>.Success(user);
        }
    }
}
