
using learning_center_webapi.Contexts.IAM.Application.CommandServices;
using learning_center_webapi.Contexts.IAM.Application.QueryServices;
using learning_center_webapi.Contexts.IAM.Domain.Exceptions;
using learning_center_webapi.Contexts.IAM.Domain.Queries;
using learning_center_webapi.Contexts.IAM.Interfaces.REST.Resources;
using learning_center_webapi.Contexts.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_webapi.Contexts.IAM.Interfaces.REST;

[Route("api/users")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthCommandService _authCommandService;
    private readonly UserQueryService _userQueryService;

    public AuthController(AuthCommandService authCommandService, UserQueryService userQueryService)
    {
        _authCommandService = authCommandService;
        _userQueryService = userQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> SignIn([FromQuery] string email, [FromQuery] string password)
    {
        try
        {
            var command = new SignInResource { Email = email, Password = password };
            var signInCommand = SignInCommandFromResourceAssembler.ToCommand(command);
            var user = await _authCommandService.Handle(signInCommand);
            var resource = AuthResponseResourceFromEntityAssembler.ToResource(user);
            
            return Ok(new[] { resource });
        }
        catch (InvalidCredentialsException)
        {
            return Ok(Array.Empty<AuthResponseResource>()); // Frontend espera array vacío en credenciales inválidas
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource resource)
    {
        try
        {
            var command = SignUpCommandFromResourceAssembler.ToCommand(resource);
            var user = await _authCommandService.Handle(command);
            var result = AuthResponseResourceFromEntityAssembler.ToResource(user);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (UserAlreadyExistsException ex)
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var query = new GetUserByIdQuery(id);
            var user = await _userQueryService.Handle(query);

            if (user == null)
                return NotFound();

            var resource = UserResourceFromEntityAssembler.ToResource(user);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllUsersQuery();
            var users = await _userQueryService.Handle(query);
            var resources = users.Select(UserResourceFromEntityAssembler.ToResource);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}