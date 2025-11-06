using learning_center_webapi.Contexts.Claims.Domain.Model.Entities;
using learning_center_webapi.Contexts.Claims.Domain.Queries;
using learning_center_webapi.Contexts.Claims.Domain.Repositories;

namespace learning_center_webapi.Contexts.Claims.Application.QueryServices;

public class ClaimQueryService(IClaimRepository claimRepository) : IClaimQueryService
{
    public async Task<IEnumerable<Claim>> Handle(GetAllClaimsQuery query)
    {
        return await claimRepository.SearchAsync(query.Status, query.UserId);
    }

    public async Task<IEnumerable<Claim>> Handle(GetClaimsByUserIdQuery query)
    {
        return await claimRepository.FindByUserIdAsync(query.UserId);
    }

    public async Task<Claim?> Handle(GetClaimByIdQuery query)
    {
        return await claimRepository.FindWithDocumentsByIdAsync(query.ClaimId);
    }
}
