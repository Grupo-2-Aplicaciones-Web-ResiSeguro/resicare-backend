using System.Net;
using learning_center_webapi.Contexts.Reminders.Application.CommandServices;
using learning_center_webapi.Contexts.Reminders.Application.QueryServices;
using learning_center_webapi.Contexts.Reminders.Domain.Commands;
using learning_center_webapi.Contexts.Reminders.Domain.Queries;
using learning_center_webapi.Contexts.Reminders.Interfaces.REST.Resources;
using learning_center_webapi.Contexts.Reminders.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_webapi.Contexts.Reminders.Interfaces;

/// <summary>
/// Controller for managing reminders.
/// </summary>
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
    [ProducesResponseType(typeof(IEnumerable<ReminderResource>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll([FromQuery] int? userId)
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

    /// <summary>
    /// Retrieves a reminder by its identifier.
    /// </summary>
    /// <param name="id">The reminder identifier.</param>
    /// <returns>The reminder details.</returns>
    /// <response code="200">Returns the reminder.</response>
    /// <response code="404">If the reminder is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReminderResource), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetReminderByIdQuery(id);
        var reminder = await _queryService.Handle(query);

        if (reminder == null)
            return NotFound(new { message = $"Reminder with id {id} not found" });

        var resource = ReminderResourceFromEntityAssembler.ToResource(reminder);
        return Ok(resource);
    }

    /// <summary>
    /// Retrieves upcoming reminders for a user (within next 24 hours).
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of upcoming reminders.</returns>
    /// <response code="200">Returns the list of upcoming reminders.</response>
    [HttpGet("upcoming")]
    [ProducesResponseType(typeof(IEnumerable<ReminderResource>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetUpcoming([FromQuery] int userId)
    {
        var query = new GetUpcomingRemindersQuery(userId);
        var reminders = await _queryService.Handle(query);
        var resources = reminders.Select(ReminderResourceFromEntityAssembler.ToResource);
        return Ok(resources);
    }

    /// <summary>
    /// Creates a new reminder.
    /// </summary>
    /// <param name="resource">The reminder creation data.</param>
    /// <returns>The created reminder.</returns>
    /// <response code="201">Returns the newly created reminder.</response>
    /// <response code="400">If the request is invalid or business rules are violated.</response>
    /// <response code="409">If a duplicate reminder exists.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ReminderResource), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateReminderResource resource)
    {
        var command = CreateReminderCommandFromResourceAssembler.ToCommand(resource);
        var reminder = await _commandService.Handle(command);
        var result = ReminderResourceFromEntityAssembler.ToResource(reminder);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing reminder.
    /// </summary>
    /// <param name="id">The reminder identifier.</param>
    /// <param name="resource">The reminder update data.</param>
    /// <returns>The updated reminder.</returns>
    /// <response code="200">Returns the updated reminder.</response>
    /// <response code="400">If the request is invalid or business rules are violated.</response>
    /// <response code="404">If the reminder is not found.</response>
    /// <response code="409">If a duplicate reminder exists.</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(ReminderResource), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReminderResource resource)
    {
        var command = UpdateReminderCommandFromResourceAssembler.ToCommand(id, resource);
        var reminder = await _commandService.Handle(command);
        var result = ReminderResourceFromEntityAssembler.ToResource(reminder);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a reminder by its identifier.
    /// </summary>
    /// <param name="id">The reminder identifier.</param>
    /// <returns>No content.</returns>
    /// <response code="204">If the reminder was successfully deleted.</response>
    /// <response code="404">If the reminder is not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteReminderCommand(id);
        await _commandService.Handle(command);
        return NoContent();
    }
}