using Application.Comments.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Azure;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments;

public class CreateComment
{
    public class Command : IRequest<Result<CommentDto>>
    {
        public Guid ActivityId { get; set; }
        public required string Body { get; set; }
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor,
        IMapper mapper) : IRequestHandler<Command, Result<CommentDto>?>
    {
        public async Task<Result<CommentDto>?> Handle(Command request, 
            CancellationToken cancellationToken)
        {
            var activity = await context.Activities.FindAsync(request.ActivityId);

            if (activity is null) return null;

            var user = await context.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == userAccesor.GetUsername());

            if (user is null) return null;

            var comment = new Comment
            {
                Author = user,
                Activity = activity,
                Body = request.Body
            };

            activity.Comments.Add(comment);

            var result = await context.SaveChangesAsync() > 0;

            return result
                ? Result<CommentDto>.Success(mapper.Map<CommentDto>(comment))
                : Result<CommentDto>.Failure("Failed to add comment.");
        }
    }
}
