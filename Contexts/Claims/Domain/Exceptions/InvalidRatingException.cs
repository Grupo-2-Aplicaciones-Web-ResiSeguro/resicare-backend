// Contexts/Claims/Domain/Exceptions/InvalidRatingException.cs
namespace learning_center_webapi.Contexts.Claims.Domain.Exceptions;

/// <summary>
/// Exception thrown when the rating value is outside the valid range.
/// </summary>
public class InvalidRatingException : Exception
{
    public int? ProvidedRating { get; }
    public int MinRating { get; }
    public int MaxRating { get; }

    public InvalidRatingException(int? providedRating, int minRating = 1, int maxRating = 5)
        : base($"Rating must be between {minRating} and {maxRating}")
    {
        ProvidedRating = providedRating;
        MinRating = minRating;
        MaxRating = maxRating;
    }
}