using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class DeletePhoto
{
    public class Command(string publicId) : IRequest<Result<Unit>>
    {
        public string PublicId { get; set; } = publicId;
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor,
        IPhotoAccessor photoAccessor) : IRequestHandler<Command, Result<Unit>?>
    {
        public async Task<Result<Unit>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == userAccesor.GetUsername());

            if (user is null) return null;

            var photo = user.Photos.FirstOrDefault(p => p.Id == request.PublicId);

            if (photo is null) return null;

            if (photo.IsMain) return Result<Unit>.Failure("Can't delete main photo.");

            var photoResult = await photoAccessor.DeletePhotoAsync(photo.Id);

            if (photoResult is null) 
                return Result<Unit>.Failure("Problem deleting photo from Cloudinary.");

            context.Photos.Remove(photo);

            var result = await context.SaveChangesAsync() > 0;

            return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Problem deleting photo from API.");
        }
    }
}
