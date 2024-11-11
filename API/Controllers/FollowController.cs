using Application.Followers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class FollowController(IMediator mediator) : BaseApiController
{
    [HttpPost("{username}")]
    public async Task<IActionResult> FollowToggle(string username)
    {
        return HandleResult(await mediator.Send(new FollowToggle.Command(username)));
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetFollowings(string username, string predicate)
    {
        return HandleResult(await mediator.Send(new GetUsersFollowing.Query
        {
            UserName = username,
            Predicate = predicate
        }));
    }
}
