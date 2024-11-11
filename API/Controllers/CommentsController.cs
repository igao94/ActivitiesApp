using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CommentsController(IMediator mediator) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> AddComment(CreateComment.Command command)
    {
        return HandleResult(await mediator.Send(command));
    }

    [HttpGet("{activityId}")]
    public async Task<IActionResult> GetComments(Guid activityId)
    {
        return HandleResult(await mediator.Send(new GetComments.Query(activityId)));
    }
}
