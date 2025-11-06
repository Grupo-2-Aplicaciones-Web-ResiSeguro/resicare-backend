using System;
using System.Linq;
using System.Threading.Tasks;
using learning_center_webapi.Contexts.Reminders.Application.CommandServices;
using learning_center_webapi.Contexts.Reminders.Application.QueryServices;
using learning_center_webapi.Contexts.Reminders.Domain.Commands;
using learning_center_webapi.Contexts.Reminders.Domain.Exceptions;
using learning_center_webapi.Contexts.Reminders.Domain.Queries;
using learning_center_webapi.Contexts.Reminders.Interfaces.REST.Resources;
using learning_center_webapi.Contexts.Reminders.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_webapi.Contexts.Reminders.Interfaces;

[Route("api/reminders")]
[ApiController]
public class ReminderController : ControllerBase
{
    private readonly ReminderCommandService _commandService;
    private readonly ReminderQueryService _queryService;

    public ReminderController(ReminderCommandService commandService, ReminderQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? userId)
    {
        try
        {
            if (userId.HasValue)
            {
                var query = new GetRemindersByUserIdQuery(userId.Value);
                var reminders = await _queryService.Handle(query);
                var resources = reminders.Select(ReminderResourceFromEntityAssembler.ToResource);
                return Ok(resources);
            }
            else
            {
                var query = new GetAllRemindersQuery();
                var reminders = await _queryService.Handle(query);
                var resources = reminders.Select(ReminderResourceFromEntityAssembler.ToResource);
                return Ok(resources);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var query = new GetReminderByIdQuery(id);
            var reminder = await _queryService.Handle(query);

            if (reminder == null)
                return NotFound();

            var resource = ReminderResourceFromEntityAssembler.ToResource(reminder);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming([FromQuery] int userId)
    {
        try
        {
            var query = new GetUpcomingRemindersQuery(userId);
            var reminders = await _queryService.Handle(query);
            var resources = reminders.Select(ReminderResourceFromEntityAssembler.ToResource);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReminderResource resource)
    {
        try
        {
            var command = CreateReminderCommandFromResourceAssembler.ToCommand(resource);
            var reminder = await _commandService.Handle(command);
            var result = ReminderResourceFromEntityAssembler.ToResource(reminder);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ReminderInPastException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReminderResource resource)
    {
        try
        {
            var command = UpdateReminderCommandFromResourceAssembler.ToCommand(id, resource);
            var reminder = await _commandService.Handle(command);
            var result = ReminderResourceFromEntityAssembler.ToResource(reminder);
            return Ok(result);
        }
        catch (ReminderNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteReminderCommand(id);
            var success = await _commandService.Handle(command);
            
            if (!success)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}