// Contexts/Claims/Interfaces/REST/ClaimController.cs
using System.Linq;
using System.Net;
using learning_center_webapi.Contexts.Claims.Application.CommandServices;
using learning_center_webapi.Contexts.Claims.Application.QueryServices;
using learning_center_webapi.Contexts.Claims.Domain.Queries;
using learning_center_webapi.Contexts.Claims.Interfaces.REST.Resources;
using learning_center_webapi.Contexts.Claims.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_webapi.Contexts.Claims.Interfaces.REST;

/// <summary>
/// Controller for managing insurance claims.
/// </summary>
[Route("api/claims")]
[ApiController]
public class ClaimController(IClaimQueryService queryService, IClaimCommandService commandService) : ControllerBase
{
    /// <summary>
    /// Retrieves all claims with optional filters.
    /// </summary>
    /// <param name="status">Optional status filter.</param>
    /// <param name="userId">Optional user identifier filter.</param>
    /// <returns>A collection of claims.</returns>
    /// <response code="200">Returns the list of claims.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClaimResource>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] int? userId)
    {
        var query = new GetAllClaimsQuery(status, userId);
        var claims = await queryService.Handle(query);
        var resources = claims.Select(ClaimResourceFromEntityAssembler.ToResource).ToList();
        return Ok(resources);
    }

    /// <summary>
    /// Retrieves all claims for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of claims belonging to the user.</returns>
    /// <response code="200">Returns the list of user claims.</response>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ClaimResource>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var query = new GetClaimsByUserIdQuery(userId);
        var claims = await queryService.Handle(query);
        var resources = claims.Select(ClaimResourceFromEntityAssembler.ToResource).ToList();
        return Ok(resources);
    }

    /// <summary>
    /// Retrieves a claim by its identifier.
    /// </summary>
    /// <param name="id">The claim identifier.</param>
    /// <returns>The claim details.</returns>
    /// <response code="200">Returns the claim.</response>
    /// <response code="404">If the claim is not found.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClaimResource), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetClaimByIdQuery(id);
        var claim = await queryService.Handle(query);
        if (claim == null) return NotFound(new { message = $"Claim with ID {id} not found" });
        var resource = ClaimResourceFromEntityAssembler.ToResource(claim);
        return Ok(resource);
    }

    /// <summary>
    /// Creates a new claim.
    /// </summary>
    /// <param name="resource">The claim creation data.</param>
    /// <returns>The created claim.</returns>
    /// <response code="201">Returns the newly created claim.</response>
    /// <response code="400">If the request is invalid or business rules are violated.</response>
    /// <response code="409">If an active claim already exists for the registered object.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ClaimResource), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateClaimResource resource)
    {
        var command = CreateClaimCommandFromResourceAssembler.ToCommand(resource);
        var claim = await commandService.Handle(command);
        var claimResource = ClaimResourceFromEntityAssembler.ToResource(claim);
        return CreatedAtAction(nameof(GetById), new { id = claim.Id }, claimResource);
    }

    /// <summary>
    /// Updates an existing claim.
    /// </summary>
    /// <param name="id">The claim identifier.</param>
    /// <param name="resource">The claim update data.</param>
    /// <returns>The updated claim.</returns>
    /// <response code="200">Returns the updated claim.</response>
    /// <response code="400">If the request is invalid or business rules are violated.</response>
    /// <response code="404">If the claim is not found.</response>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(ClaimResource), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClaimResource resource)
    {
        var command = UpdateClaimCommandFromResourceAssembler.ToCommand(id, resource);
        var updatedClaim = await commandService.Handle(command);
        var claimResource = ClaimResourceFromEntityAssembler.ToResource(updatedClaim);
        return Ok(claimResource);
    }

    /// <summary>
    /// Deletes a claim by its identifier.
    /// </summary>
    /// <param name="id">The claim identifier.</param>
    /// <returns>No content.</returns>
    /// <response code="204">If the claim was successfully deleted.</response>
    /// <response code="404">If the claim is not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await commandService.DeleteAsync(id);
        return NoContent();
    }
}