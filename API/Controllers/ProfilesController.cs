using Application.Profiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfilesController(IMediator mediator) : BaseApiController
{
    [HttpGet("{username}")]
    public async Task<IActionResult> GetProfile(string username)
    {
        return HandleResult(await mediator.Send(new GetUserProfile.Query(username)));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(EditUserProfile.Command command)
    {
        return HandleResult(await mediator.Send(command));
    }
}
