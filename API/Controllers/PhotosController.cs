using Application.Photos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PhotosController(IMediator mediator) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> AddPhoto([FromForm] AddPhoto.Command command)
    {
        return HandleResult(await mediator.Send(command));
    }

    [HttpDelete("{publicId}")]
    public async Task<IActionResult> DeletePhoto(string publicId)
    {
        return HandleResult(await mediator.Send(new DeletePhoto.Command(publicId)));
    }

    [HttpPost("{publicId}/setMain")]
    public async Task<IActionResult> SetMainPhoto(string publicId)
    {
        return HandleResult(await mediator.Send(new SetMainPhoto.Command(publicId)));
    }
}
