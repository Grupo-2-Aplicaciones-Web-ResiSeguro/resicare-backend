// Contexts/Claims/Application/CommandServices/ClaimCommandService.cs
using System.Linq;
using learning_center_webapi.Contexts.Claims.Domain.Commands;
using learning_center_webapi.Contexts.Claims.Domain.Exceptions;
using learning_center_webapi.Contexts.Claims.Domain.Model.Entities;
using learning_center_webapi.Contexts.Claims.Domain.Repositories;
using learning_center_webapi.Contexts.Shared.Domain.Repositories;
using learning_center_webapi.Contexts.IAM.Interfaces.REST.ACL;
using learning_center_webapi.Contexts.RegisteredObjects.Interfaces.REST.ACL;

namespace learning_center_webapi.Contexts.Claims.Application.CommandServices;

/// <summary>
/// Service responsible for handling claim-related commands.
/// </summary>
public class ClaimCommandService(
    IClaimRepository claimRepository,
    IUnitOfWork unitOfWork,
    IUserFacade userFacade,
    IRegisteredObjectFacade registeredObjectFacade) : IClaimCommandService
{
    private const int MaxReportingPeriodDays = 30;
    private const int MinRating = 1;
    private const int MaxRating = 5;

    /// <summary>
    /// Handles the creation of a new claim.
    /// </summary>
    /// <param name="command">Command containing claim creation data.</param>
    /// <returns>The created claim entity.</returns>
    /// <exception cref="ClaimOutsideReportingPeriodException">Thrown when incident date exceeds reporting period.</exception>
    /// <exception cref="DuplicateActiveClaimException">Thrown when an active claim already exists for the registered object.</exception>
    public async Task<Claim> Handle(CreateClaimCommand command)
    {
        // Validate incident date is within reporting period
        ValidateIncidentDate(command.IncidentDate);

        // Validate user exists using ACL facade
        var isValidUser = await userFacade.IsValidUserId(command.UserId);
        if (!isValidUser)
        {
            throw new ArgumentException($"User with id {command.UserId} does not exist.");
        }

        // Validate no active claim exists for the registered object
        if (command.RegisteredObjectId.HasValue)
        {
            // Validate registered object exists using ACL facade
            var isValidObject = await registeredObjectFacade.IsValidRegisteredObjectId(command.RegisteredObjectId.Value);
            if (!isValidObject)
            {
                throw new ArgumentException($"Registered object with id {command.RegisteredObjectId.Value} does not exist.");
            }

            await ValidateNoDuplicateActiveClaim(command.RegisteredObjectId.Value);
        }

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

    /// <summary>
    /// Handles the update of an existing claim.
    /// </summary>
    /// <param name="command">Command containing claim update data.</param>
    /// <returns>The updated claim entity, or null if not found.</returns>
    /// <exception cref="ClaimNotFoundException">Thrown when the claim is not found.</exception>
    /// <exception cref="InvalidClaimStatusTransitionException">Thrown when status transition is invalid.</exception>
    /// <exception cref="InvalidRatingException">Thrown when rating is outside valid range.</exception>
    public async Task<Claim?> Handle(UpdateClaimCommand command)
    {
        var existingClaim = await claimRepository.FindWithDocumentsByIdAsync(command.ClaimId);
        if (existingClaim == null)
            throw new ClaimNotFoundException(command.ClaimId);

        // Validate status transition if status is being changed
        if (!string.IsNullOrWhiteSpace(command.Status) && command.Status != existingClaim.Status)
        {
            ValidateStatusTransition(existingClaim.Status, command.Status);
            existingClaim.Status = command.Status;
        }

        // Validate rating if provided
        if (command.Rating.HasValue)
        {
            ValidateRating(command.Rating.Value);
            existingClaim.Rating = command.Rating;
        }

        if (!string.IsNullOrWhiteSpace(command.Description))
            existingClaim.Description = command.Description;

        if (command.RegisteredObjectId.HasValue)
            existingClaim.RegisteredObjectId = command.RegisteredObjectId;

        existingClaim.UpdatedDate = DateTime.UtcNow;

        claimRepository.Update(existingClaim);
        await unitOfWork.CompleteAsync();
        return existingClaim;
    }

    /// <summary>
    /// Deletes a claim by its identifier.
    /// </summary>
    /// <param name="claimId">The claim identifier.</param>
    /// <returns>True if deleted successfully, false otherwise.</returns>
    /// <exception cref="ClaimNotFoundException">Thrown when the claim is not found.</exception>
    public async Task<bool> DeleteAsync(int claimId)
    {
        var existingClaim = await claimRepository.FindWithDocumentsByIdAsync(claimId);
        if (existingClaim == null)
            throw new ClaimNotFoundException(claimId);

        claimRepository.Remove(existingClaim);
        await unitOfWork.CompleteAsync();
        return true;
    }

    /// <summary>
    /// Validates that the incident date is within the allowed reporting period.
    /// </summary>
    /// <param name="incidentDate">The incident date to validate.</param>
    /// <exception cref="ClaimOutsideReportingPeriodException">Thrown when date exceeds reporting period.</exception>
    private static void ValidateIncidentDate(DateTime incidentDate)
    {
        var daysDifference = (DateTime.UtcNow.Date - incidentDate.Date).Days;
        if (daysDifference > MaxReportingPeriodDays)
        {
            throw new ClaimOutsideReportingPeriodException(incidentDate, MaxReportingPeriodDays);
        }

        // Prevent future dates
        if (incidentDate.Date > DateTime.UtcNow.Date)
        {
            throw new ClaimOutsideReportingPeriodException(incidentDate, MaxReportingPeriodDays);
        }
    }

    /// <summary>
    /// Validates that no active claim exists for the registered object.
    /// </summary>
    /// <param name="registeredObjectId">The registered object identifier.</param>
    /// <exception cref="DuplicateActiveClaimException">Thrown when an active claim exists.</exception>
    private async Task ValidateNoDuplicateActiveClaim(int registeredObjectId)
    {
        var activeClaim = await claimRepository.FindActiveClaimByRegisteredObjectIdAsync(registeredObjectId);
        if (activeClaim != null)
        {
            throw new DuplicateActiveClaimException(registeredObjectId, activeClaim.Id);
        }
    }

    /// <summary>
    /// Validates claim status transitions according to business rules.
    /// </summary>
    /// <param name="currentStatus">Current claim status.</param>
    /// <param name="newStatus">New desired status.</param>
    /// <exception cref="InvalidClaimStatusTransitionException">Thrown when transition is invalid.</exception>
    private static void ValidateStatusTransition(string currentStatus, string newStatus)
    {
        var validTransitions = new Dictionary<string, string[]>
        {
            { "pending", new[] { "in_review", "rejected" } },
            { "in_review", new[] { "approved", "rejected" } },
            { "approved", Array.Empty<string>() },
            { "rejected", Array.Empty<string>() }
        };

        if (!validTransitions.TryGetValue(currentStatus, out var allowedStatuses) ||
            !allowedStatuses.Contains(newStatus))
        {
            throw new InvalidClaimStatusTransitionException(currentStatus, newStatus);
        }
    }

    /// <summary>
    /// Validates that the rating is within the valid range.
    /// </summary>
    /// <param name="rating">The rating value to validate.</param>
    /// <exception cref="InvalidRatingException">Thrown when rating is outside valid range.</exception>
    private static void ValidateRating(int rating)
    {
        if (rating < MinRating || rating > MaxRating)
        {
            throw new InvalidRatingException(rating, MinRating, MaxRating);
        }
    }

    /// <summary>
    /// Generates a unique claim number.
    /// </summary>
    /// <returns>A unique claim number in the format CLM-{timestamp}-{suffix}.</returns>
    private static string GenerateClaimNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var suffix = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant();
        return $"CLM-{timestamp}-{suffix}";
    }
}