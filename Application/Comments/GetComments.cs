using Application.Comments.DTOs;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments;

public class GetComments
{
    public class Query(Guid activityId) : IRequest<Result<List<CommentDto>>>
    {
        public Guid ActivityId { get; set; } = activityId;
    }

    public class Handler(DataContext context,
        IMapper mapper) : IRequestHandler<Query, Result<List<CommentDto>>>
    {
        public async Task<Result<List<CommentDto>>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var comments = await context.Comments
                .Where(c => c.Activity.Id == request.ActivityId)
                .OrderByDescending(c => c.CreatedAt)
                .ProjectTo<CommentDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            return Result<List<CommentDto>>.Success(comments);
        }
    }
}
