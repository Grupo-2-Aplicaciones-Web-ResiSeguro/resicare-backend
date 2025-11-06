using System.Linq;
using learning_center_webapi.Contexts.Claims.Application.CommandServices;
using learning_center_webapi.Contexts.Claims.Application.QueryServices;
using learning_center_webapi.Contexts.Claims.Domain.Queries;
using learning_center_webapi.Contexts.Claims.Interfaces.REST.Resources;
using learning_center_webapi.Contexts.Claims.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_webapi.Contexts.Claims.Interfaces.REST;

[Route("api/claims")]
[ApiController]
public class ClaimController(IClaimQueryService queryService, IClaimCommandService commandService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] int? userId)
    {
        var query = new GetAllClaimsQuery(status, userId);
        var claims = await queryService.Handle(query);
        var resources = claims.Select(ClaimResourceFromEntityAssembler.ToResource).ToList();
        return Ok(resources);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var query = new GetClaimsByUserIdQuery(userId);
        var claims = await queryService.Handle(query);
        var resources = claims.Select(ClaimResourceFromEntityAssembler.ToResource).ToList();
        return Ok(resources);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetClaimByIdQuery(id);
        var claim = await queryService.Handle(query);
        if (claim == null) return NotFound();
        var resource = ClaimResourceFromEntityAssembler.ToResource(claim);
        return Ok(resource);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClaimResource resource)
    {
        var command = CreateClaimCommandFromResourceAssembler.ToCommand(resource);
        var claim = await commandService.Handle(command);
        var claimResource = ClaimResourceFromEntityAssembler.ToResource(claim);
        return CreatedAtAction(nameof(GetById), new { id = claim.Id }, claimResource);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClaimResource resource)
    {
        var command = UpdateClaimCommandFromResourceAssembler.ToCommand(id, resource);
        var updatedClaim = await commandService.Handle(command);
        if (updatedClaim == null) return NotFound();
        var claimResource = ClaimResourceFromEntityAssembler.ToResource(updatedClaim);
        return Ok(claimResource);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await commandService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
