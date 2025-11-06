using System.Linq;
using learning_center_webapi.Contexts.Claims.Domain.Commands;
using learning_center_webapi.Contexts.Claims.Domain.Model.Entities;
using learning_center_webapi.Contexts.Claims.Domain.Repositories;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;

namespace learning_center_webapi.Contexts.Claims.Application.CommandServices;

public class ClaimCommandService(IClaimRepository claimRepository, IUnitOfWork unitOfWork) : IClaimCommandService
{
    public async Task<Claim> Handle(CreateClaimCommand command)
    {
        var claim = new Claim
        {
            Number = GenerateClaimNumber(),
            Type = command.Type,
            Status = "pending",
            IncidentDate = command.IncidentDate,
            Description = command.Description,
            RegisteredObjectId = command.RegisteredObjectId,
            UserId = command.UserId,
            CreatedDate = DateTime.UtcNow,
            Documents = command.Documents
                .Select(document => new ClaimDocument
                {
                    Name = document.Name,
                    ContentType = document.ContentType,
                    Size = document.Size,
                    Data = document.Data
                }).ToList()
        };

        await claimRepository.AddAsync(claim);
        await unitOfWork.CompleteAsync();
        return claim;
    }

    public async Task<Claim?> Handle(UpdateClaimCommand command)
    {
        var existingClaim = await claimRepository.FindWithDocumentsByIdAsync(command.ClaimId);
        if (existingClaim == null) return null;

        if (!string.IsNullOrWhiteSpace(command.Status)) existingClaim.Status = command.Status;
        if (!string.IsNullOrWhiteSpace(command.Description)) existingClaim.Description = command.Description;
        if (command.RegisteredObjectId.HasValue) existingClaim.RegisteredObjectId = command.RegisteredObjectId;
        if (command.Rating.HasValue) existingClaim.Rating = command.Rating;
        existingClaim.UpdatedDate = DateTime.UtcNow;

        claimRepository.Update(existingClaim);
        await unitOfWork.CompleteAsync();
        return existingClaim;
    }

    public async Task<bool> DeleteAsync(int claimId)
    {
        var existingClaim = await claimRepository.FindWithDocumentsByIdAsync(claimId);
        if (existingClaim == null) return false;
        claimRepository.Remove(existingClaim);
        await unitOfWork.CompleteAsync();
        return true;
    }

    private static string GenerateClaimNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var suffix = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
        return $"CLM-{timestamp}-{suffix}";
    }
}
