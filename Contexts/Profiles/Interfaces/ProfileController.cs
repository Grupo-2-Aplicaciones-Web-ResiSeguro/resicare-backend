
using learning_center_webapi.Contexts.Profiles.Application.CommandServices;
using learning_center_webapi.Contexts.Profiles.Application.QueryServices;
using learning_center_webapi.Contexts.Profiles.Domain.Commands;
using learning_center_webapi.Contexts.Profiles.Domain.Exceptions;
using learning_center_webapi.Contexts.Profiles.Domain.Queries;
using learning_center_webapi.Contexts.Profiles.Interfaces.REST.Resources;
using learning_center_webapi.Contexts.Profiles.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_webapi.Contexts.Profiles.Interfaces.REST;

[Route("api/profiles")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly ProfileCommandService _commandService;
    private readonly ProfileQueryService _queryService;

    public ProfileController(ProfileCommandService commandService, ProfileQueryService queryService)
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
                // GET /api/profiles?userId=X
                var query = new GetProfileByUserIdQuery(userId.Value);
                var profile = await _queryService.Handle(query);
                
                if (profile == null)
                    return Ok(Array.Empty<ProfileResource>()); // Frontend espera array
                
                var resource = ProfileResourceFromEntityAssembler.ToResource(profile);
                return Ok(new[] { resource }); // Retornar como array
            }
            else
            {
                var query = new GetAllProfilesQuery();
                var profiles = await _queryService.Handle(query);
                var resources = profiles.Select(ProfileResourceFromEntityAssembler.ToResource);
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
            var query = new GetProfileByIdQuery(id);
            var profile = await _queryService.Handle(query);

            if (profile == null)
                return NotFound();

            var resource = ProfileResourceFromEntityAssembler.ToResource(profile);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProfileResource resource)
    {
        try
        {
            var command = CreateProfileCommandFromResourceAssembler.ToCommand(resource);
            var profile = await _commandService.Handle(command);
            var result = ProfileResourceFromEntityAssembler.ToResource(profile);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ProfileAlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message });
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
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProfileResource resource)
    {
        try
        {
            var command = UpdateProfileCommandFromResourceAssembler.ToCommand(id, resource);
            var profile = await _commandService.Handle(command);
            var result = ProfileResourceFromEntityAssembler.ToResource(profile);
            return Ok(result);
        }
        catch (ProfileNotFoundException)
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
            var command = new DeleteProfileCommand(id);
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