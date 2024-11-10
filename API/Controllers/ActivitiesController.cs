using Application.Activities;
using Application.Activities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController(IMediator mediator) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<ActivityDto>>> GetActivites()
    {
        return HandleResult(await mediator.Send(new GetActivities.Query()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityDto>> GetActivity(Guid id)
    {
        return HandleResult(await mediator.Send(new GetActivity.Query(id)));
    }

    [HttpPost]
    public async Task<ActionResult<ActivityDto>> CreateActivity(CreateActivityDto createActivityDto)
    {
        return HandleResult(await mediator.Send(new CreateActivity.Command(createActivityDto)));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivity(Guid id, EditActivityDto editActivityDto)
    {
        editActivityDto.Id = id;

        return HandleResult(await mediator.Send(new EditActivity.Command(editActivityDto)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
        return HandleResult(await mediator.Send(new DeleteActivity.Command(id)));
    }
}
