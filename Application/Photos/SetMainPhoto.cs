using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class SetMainPhoto
{
    public class Command(string publicId) : IRequest<Result<Unit>>
    {
        public string PublicId { get; set; } = publicId;
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor) : IRequestHandler<Command, Result<Unit>?>
    {
        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == userAccesor.GetUsername());

            if (user is null) return null;

            var photo = user.Photos.FirstOrDefault(p => p.Id == request.PublicId);

            if (photo is null) return null;

            if (photo.IsMain) return Result<Unit>.Failure("Already main photo.");

            var currentMainPhoto = user.Photos.FirstOrDefault(p => p.IsMain);

            if (currentMainPhoto is not null) currentMainPhoto.IsMain = false;

            photo.IsMain = true;

            var result = await context.SaveChangesAsync() > 0;

            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Problem setting main photo.");
        }
    }
}
