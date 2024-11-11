using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class GetUserProfile
{
    public class Query(string username) : IRequest<Result<Profile>>
    {
        public string Username { get; set; } = username;
    }

    public class Handler(DataContext context,
        IMapper mapper) : IRequestHandler<Query, Result<Profile>?>
    {
        public async Task<Result<Profile>?> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .ProjectTo<Profile>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user is null) return null;

            return Result<Profile>.Success(user);
        }
    }
}
