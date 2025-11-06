using learning_center_webapi.Contexts.Claims.Domain.Commands;
using learning_center_webapi.Contexts.Claims.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.Claims.Application.CommandServices;

public interface IClaimCommandService
{
    Task<Claim> Handle(CreateClaimCommand command);
    Task<Claim?> Handle(UpdateClaimCommand command);
    Task<bool> DeleteAsync(int claimId);
}
