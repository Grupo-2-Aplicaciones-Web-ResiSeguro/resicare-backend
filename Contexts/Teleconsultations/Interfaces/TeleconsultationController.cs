// Contexts/Teleconsultations/Interfaces/REST/TeleconsultationController.cs
using System.Net;
using learning_center_webapi.Contexts.Teleconsultations.Application.CommandServices;
using learning_center_webapi.Contexts.Teleconsultations.Application.QueryServices;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Commands;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Queries;
using learning_center_webapi.Contexts.Teleconsultations.Interfaces.REST.Resources;
using learning_center_webapi.Contexts.Teleconsultations.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_webapi.Contexts.Teleconsultations.Interfaces;

/// <summary>
/// Controller for managing teleconsultations.
/// </summary>
[Route("api/teleconsultations")]
[ApiController]
public class TeleconsultationController : ControllerBase
{
    private readonly TeleconsultationCommandService _commandService;
    private readonly TeleconsultationQueryService _queryService;

    public TeleconsultationController(
        TeleconsultationCommandService commandService,
        TeleconsultationQueryService queryService)
    {
        _commandService = commandService;
        _queryService = queryService;
    }

    /// <summary>
    /// Retrieves all teleconsultations with optional user filter.
    /// </summary>
    /// <param name="userId">Optional user identifier filter.</param>
    /// <returns>A collection of teleconsultations.</returns>
    /// <response code="200">Returns the list of teleconsultations.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TeleconsultationResource>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll([FromQuery] int? userId)
    {
        if (userId.HasValue)
        {
            var query = new GetTeleconsultationsByUserId(userId.Value);
            var teleconsultations = await _queryService.Handle(query);
            var resources = teleconsultations.Select(TeleconsultationResourceFromEntityAssembler.ToResource);
            return Ok(resources);
        }
        else
        {
            var query = new GetAllTeleconsultations();
            var teleconsultations = await _queryService.Handle(query);
            var resources = teleconsultations.Select(TeleconsultationResourceFromEntityAssembler.ToResource);
            return Ok(resources);
        }
    }

    /// <summary>
    /// Retrieves a teleconsultation by its identifier.
    /// </summary>
    /// <param name="id">The teleconsultation identifier.</param>
    /// <returns>The teleconsultation details.</returns>
    /// <response code="200">Returns the teleconsultation.</response>
    /// <response code="404">If the teleconsultation is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TeleconsultationResource), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetTeleconsultationById(id);
        var teleconsultation = await _queryService.Handle(query);

        if (teleconsultation == null)
            return NotFound(new { message = $"Teleconsultation with id {id} not found" });

        var resource = TeleconsultationResourceFromEntityAssembler.ToResource(teleconsultation);
        return Ok(resource);
    }

    /// <summary>
    /// Retrieves teleconsultations by service type.
    /// </summary>
    /// <param name="service">The service type.</param>
    /// <returns>A collection of teleconsultations for the specified service.</returns>
    /// <response code="200">Returns the list of teleconsultations.</response>
    [HttpGet("service/{service}")]
    [ProducesResponseType(typeof(IEnumerable<TeleconsultationResource>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByService(string service)
    {
        var query = new GetTeleconsultationsByService(service);
        var teleconsultations = await _queryService.Handle(query);
        var resources = teleconsultations.Select(TeleconsultationResourceFromEntityAssembler.ToResource);
        return Ok(resources);
    }

    /// <summary>
    /// Creates a new teleconsultation.
    /// </summary>
    /// <param name="resource">The teleconsultation creation data.</param>
    /// <returns>The created teleconsultation.</returns>
    /// <response code="201">Returns the newly created teleconsultation.</response>
    /// <response code="400">If the request is invalid or business rules are violated.</response>
    /// <response code="409">If the time slot is already occupied.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TeleconsultationResource), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateTeleconsultationResource resource)
    {
        var command = CreateTeleconsultationCommandFromResourceAssembler.ToCommand(resource);
        var teleconsultation = await _commandService.Handle(command);
        var result = TeleconsultationResourceFromEntityAssembler.ToResource(teleconsultation);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing teleconsultation.
    /// </summary>
    /// <param name="id">The teleconsultation identifier.</param>
    /// <param name="resource">The teleconsultation update data.</param>
    /// <returns>The updated teleconsultation.</returns>
    /// <response code="200">Returns the updated teleconsultation.</response>
    /// <response code="400">If the request is invalid or business rules are violated.</response>
    /// <response code="404">If the teleconsultation is not found.</response>
    /// <response code="409">If the new time slot is already occupied.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TeleconsultationResource), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTeleconsultationResource resource)
    {
        var command = UpdateTeleconsultationCommandFromResourceAssembler.ToCommand(id, resource);
        var teleconsultation = await _commandService.Handle(command);
        var result = TeleconsultationResourceFromEntityAssembler.ToResource(teleconsultation);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a teleconsultation by its identifier.
    /// </summary>
    /// <param name="id">The teleconsultation identifier.</param>
    /// <returns>No content.</returns>
    /// <response code="204">If the teleconsultation was successfully deleted.</response>
    /// <response code="404">If the teleconsultation is not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteTeleconsultationCommand { Id = id };
        await _commandService.Handle(command);
        return NoContent();
    }
}