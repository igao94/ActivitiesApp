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
        return await mediator.Send(new GetActivities.Query());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityDto>> GetActivity(Guid id)
    {
        return await mediator.Send(new GetActivity.Query(id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> GetActivity(Guid id, EditActivityDto editActivityDto)
    {
        editActivityDto.Id = id;

        await mediator.Send(new EditActivity.Command(editActivityDto));

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
        await mediator.Send(new DeleteActivity.Command(id));

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<ActivityDto>> CreateActivity(CreateActivityDto createActivityDto)
    {
        return await mediator.Send(new CreateActivity.Command(createActivityDto));
    }
}
