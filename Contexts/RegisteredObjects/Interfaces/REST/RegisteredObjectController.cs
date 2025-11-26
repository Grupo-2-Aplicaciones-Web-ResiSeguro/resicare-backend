using System.Net;
using learning_center_webapi.Contexts.RegisteredObjects.Application.CommandServices;
using learning_center_webapi.Contexts.RegisteredObjects.Application.QueryServices;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Queries;
using learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Resources;
using learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST;

/// <summary>
/// Controller for managing registered objects.
/// </summary>
[Route("api/registered-objects")]
[ApiController]
public class RegisteredObjectController(IRegisteredObjectQueryService queryService, IRegisteredObjectCommandService commandService) : ControllerBase
{
    /// <summary>
    /// Retrieves all registered objects with optional search and user filters.
    /// </summary>
    /// <param name="q">Optional search text.</param>
    /// <param name="userId">Optional user identifier filter.</param>
    /// <returns>A collection of registered objects.</returns>
    /// <response code="200">Returns the list of registered objects.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RegisteredObjectResource>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? q, [FromQuery] int? userId)
    {
        var query = new GetAllRegisteredObjectsQuery(q, userId);
        var objects = await queryService.Handle(query);
        var resources = objects.Select(RegisteredObjectResourceFromEntityAssembler.ToResource).ToList();
        return Ok(resources);
    }

    /// <summary>
    /// Retrieves all registered objects for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of objects belonging to the user.</returns>
    /// <response code="200">Returns the list of user's registered objects.</response>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IEnumerable<RegisteredObjectResource>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var query = new GetRegisteredObjectsByUserIdQuery(userId);
        var objects = await queryService.Handle(query);
        var resources = objects.Select(RegisteredObjectResourceFromEntityAssembler.ToResource).ToList();
        return Ok(resources);
    }

    /// <summary>
    /// Retrieves a registered object by its identifier.
    /// </summary>
    /// <param name="id">The object identifier.</param>
    /// <returns>The registered object details.</returns>
    /// <response code="200">Returns the registered object.</response>
    /// <response code="404">If the object is not found.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RegisteredObjectResource), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetRegisteredObjectByIdQuery(id);
        var registeredObject = await queryService.Handle(query);
        if (registeredObject == null)
            return NotFound(new { message = $"Registered object with ID {id} not found" });
        var resource = RegisteredObjectResourceFromEntityAssembler.ToResource(registeredObject);
        return Ok(resource);
    }

    /// <summary>
    /// Creates a new registered object.
    /// </summary>
    /// <param name="resource">The object creation data.</param>
    /// <returns>The created registered object.</returns>
    /// <response code="201">Returns the newly created object.</response>
    /// <response code="400">If the request is invalid or business rules are violated.</response>
    /// <response code="409">If a duplicate serial number exists.</response>
    [HttpPost]
    [ProducesResponseType(typeof(RegisteredObjectResource), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateRegisteredObjectResource resource)
    {
        var command = CreateRegisteredObjectCommandFromResourceAssembler.ToCommand(resource);
        var registeredObject = await commandService.Handle(command);
        var registeredObjectResource = RegisteredObjectResourceFromEntityAssembler.ToResource(registeredObject);
        return CreatedAtAction(nameof(GetById), new { id = registeredObject.Id }, registeredObjectResource);
    }

    /// <summary>
    /// Updates an existing registered object.
    /// </summary>
    /// <param name="id">The object identifier.</param>
    /// <param name="resource">The object update data.</param>
    /// <returns>The updated registered object.</returns>
    /// <response code="200">Returns the updated object.</response>
    /// <response code="400">If the request is invalid or business rules are violated.</response>
    /// <response code="404">If the object is not found.</response>
    /// <response code="409">If a duplicate serial number exists.</response>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(RegisteredObjectResource), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRegisteredObjectResource resource)
    {
        var command = UpdateRegisteredObjectCommandFromResourceAssembler.ToCommand(id, resource);
        var updated = await commandService.Handle(command);
        var registeredObjectResource = RegisteredObjectResourceFromEntityAssembler.ToResource(updated);
        return Ok(registeredObjectResource);
    }

    /// <summary>
    /// Deletes a registered object by its identifier.
    /// </summary>
    /// <param name="id">The object identifier.</param>
    /// <returns>No content.</returns>
    /// <response code="204">If the object was successfully deleted.</response>
    /// <response code="404">If the object is not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await commandService.DeleteAsync(id);
        return NoContent();
    }
}