using learning_center_webapi.Contexts.Shared.Domain.Model.Entities;

namespace learning_center_webapi.Contexts.Claims.Domain.Model.Entities;

public class ClaimDocument : BaseEntity
{
    /// <summary>
    /// Identifier of the claim associated with this document.
    /// </summary>
    public int ClaimId { get; set; }

    /// <summary>
    /// Original file name provided by the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Media type of the document (MIME).
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// File size in bytes.
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// Base64 encoded file content.
    /// </summary>
    public string Data { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to the related claim.
    /// </summary>
    public Claim Claim { get; set; } = null!;
}
