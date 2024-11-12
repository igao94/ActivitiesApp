using Application.Activities;
using Application.Activities.DTOs;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ActivitiesController(IMediator mediator) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<ActivityDto>>> GetActivites
        ([FromQuery] ActivityParams activityParams)
    {
        return HandlePagedResult(await mediator.Send(new GetActivities.Query(activityParams)));
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

    [Authorize(Policy = SecurityConstants.IsHostRequirement)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivity(Guid id, EditActivityDto editActivityDto)
    {
        editActivityDto.Id = id;

        return HandleResult(await mediator.Send(new EditActivity.Command(editActivityDto)));
    }

    [Authorize(Policy = SecurityConstants.IsHostRequirement)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id)
    {
        return HandleResult(await mediator.Send(new DeleteActivity.Command(id)));
    }

    [HttpPost("{id}/attend")]
    public async Task<IActionResult> UpdateAttendance(Guid id)
    {
        return HandleResult(await mediator.Send(new UpdateAttendance.Command(id)));
    }
}
