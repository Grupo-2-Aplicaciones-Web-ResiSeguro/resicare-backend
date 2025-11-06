using learning_center_webapi.Contexts.Claims.Domain.Model.Entities;
using learning_center_webapi.Contexts.Claims.Domain.Queries;

namespace learning_center_webapi.Contexts.Claims.Application.QueryServices;

public interface IClaimQueryService
{
    Task<IEnumerable<Claim>> Handle(GetAllClaimsQuery query);
    Task<IEnumerable<Claim>> Handle(GetClaimsByUserIdQuery query);
    Task<Claim?> Handle(GetClaimByIdQuery query);
}
