
using learning_center_webapi.Contexts.Teleconsultations.Application.CommandServices;
using learning_center_webapi.Contexts.Teleconsultations.Application.QueryServices;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Commands;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Queries;
using learning_center_webapi.Contexts.Teleconsultations.Interfaces.REST.Resources;
using learning_center_webapi.Contexts.Teleconsultations.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_webapi.Contexts.Teleconsultations.Interfaces;

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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? userId)
    {
        try
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
            var query = new GetTeleconsultationById(id);
            var teleconsultation = await _queryService.Handle(query);

            if (teleconsultation == null)
                return NotFound();

            var resource = TeleconsultationResourceFromEntityAssembler.ToResource(teleconsultation);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("service/{service}")]
    public async Task<IActionResult> GetByService(string service)
    {
        try
        {
            var query = new GetTeleconsultationsByService(service);
            var teleconsultations = await _queryService.Handle(query);
            var resources = teleconsultations.Select(TeleconsultationResourceFromEntityAssembler.ToResource);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeleconsultationResource resource)
    {
        try
        {
            var command = CreateTeleconsultationCommandFromResourceAssembler.ToCommand(resource);
            var teleconsultation = await _commandService.Handle(command);
            var result = TeleconsultationResourceFromEntityAssembler.ToResource(teleconsultation);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTeleconsultationResource resource)
    {
        try
        {
            var command = UpdateTeleconsultationCommandFromResourceAssembler.ToCommand(id, resource);
            var teleconsultation = await _commandService.Handle(command);
            var result = TeleconsultationResourceFromEntityAssembler.ToResource(teleconsultation);
            return Ok(result);
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
            var command = new DeleteTeleconsultationCommand { Id = id };
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