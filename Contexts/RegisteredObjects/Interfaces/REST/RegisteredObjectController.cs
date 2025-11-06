using System.Linq;
using learning_center_webapi.Contexts.RegisteredObjects.Application.CommandServices;
using learning_center_webapi.Contexts.RegisteredObjects.Application.QueryServices;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Queries;
using learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Resources;
using learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST;

[Route("api/registered-objects")]
[ApiController]
public class RegisteredObjectController(IRegisteredObjectQueryService queryService, IRegisteredObjectCommandService commandService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? q, [FromQuery] int? userId)
    {
        var query = new GetAllRegisteredObjectsQuery(q, userId);
        var objects = await queryService.Handle(query);
        var resources = objects.Select(RegisteredObjectResourceFromEntityAssembler.ToResource).ToList();
        return Ok(resources);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var query = new GetRegisteredObjectsByUserIdQuery(userId);
        var objects = await queryService.Handle(query);
        var resources = objects.Select(RegisteredObjectResourceFromEntityAssembler.ToResource).ToList();
        return Ok(resources);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetRegisteredObjectByIdQuery(id);
        var registeredObject = await queryService.Handle(query);
        if (registeredObject == null) return NotFound();
        var resource = RegisteredObjectResourceFromEntityAssembler.ToResource(registeredObject);
        return Ok(resource);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRegisteredObjectResource resource)
    {
        var command = CreateRegisteredObjectCommandFromResourceAssembler.ToCommand(resource);
        var registeredObject = await commandService.Handle(command);
        var registeredObjectResource = RegisteredObjectResourceFromEntityAssembler.ToResource(registeredObject);
        return CreatedAtAction(nameof(GetById), new { id = registeredObject.Id }, registeredObjectResource);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRegisteredObjectResource resource)
    {
        var command = UpdateRegisteredObjectCommandFromResourceAssembler.ToCommand(id, resource);
        var updated = await commandService.Handle(command);
        if (updated == null) return NotFound();
        var registeredObjectResource = RegisteredObjectResourceFromEntityAssembler.ToResource(updated);
        return Ok(registeredObjectResource);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await commandService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
