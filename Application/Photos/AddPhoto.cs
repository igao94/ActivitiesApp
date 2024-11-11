using Application.Core;
using Application.Interfaces;
using Application.Photos.DTOs;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class AddPhoto
{
    public class Command : IRequest<Result<PhotoDto>>
    {
        public IFormFile File { get; set; } = null!;
    }

    public class Handler(DataContext context,
        IUserAccesor userAccesor,
        IPhotoAccessor photoAccessor,
        IMapper mapper) : IRequestHandler<Command, Result<PhotoDto>?>
    {
        public async Task<Result<PhotoDto>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == userAccesor.GetUsername());

            if (user is null) return null;

            var photoUploadResult = await photoAccessor.AddPhotoAsync(request.File);

            if (photoUploadResult is null) return null;

            var photo = new Photo
            {
                Id = photoUploadResult.PublicId,
                Url = photoUploadResult.Url,
            };

            if (!user.Photos.Any(p => p.IsMain)) photo.IsMain = true;

            user.Photos.Add(photo);

            var result = await context.SaveChangesAsync() > 0;

            return result
                ? Result<PhotoDto>.Success(mapper.Map<PhotoDto>(photo))
                : Result<PhotoDto>.Failure("Problem uploading photo.");
        }
    }
}
