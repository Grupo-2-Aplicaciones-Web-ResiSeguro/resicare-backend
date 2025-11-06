using learning_center_webapi.Contexts.Shared.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.Claims.Domain.Model.Entities;

public class Claim : BaseEntity
{
    /// <summary>
    /// Unique claim number visible to the user.
    /// </summary>
    public required string Number { get; set; }

    /// <summary>
    /// Claim type identifier (accident, theft, loss, damage).
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// Claim status (pending, in_review, approved, rejected).
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// Incident date provided by the user.
    /// </summary>
    public DateTime IncidentDate { get; set; }

    /// <summary>
    /// Detailed description of the incident.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Identifier of the registered object associated with this claim.
    /// </summary>
    public int? RegisteredObjectId { get; set; }

    /// <summary>
    /// List of documents attached to the claim.
    /// </summary>
    public ICollection<ClaimDocument> Documents { get; set; } = new List<ClaimDocument>();

    /// <summary>
    /// Identifier of the user who created the claim.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Optional user rating for the claim resolution.
    /// </summary>
    public int? Rating { get; set; }
}
