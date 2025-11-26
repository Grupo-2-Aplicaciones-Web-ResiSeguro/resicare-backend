namespace learning_center_webapi.Contexts.RegisteredObjects.Domain.Exceptions;

/// <summary>
/// Exception thrown when the object price is outside the valid range.
/// </summary>
public sealed class InvalidObjectPriceException : RegisteredObjectBusinessRuleException
{
    public decimal ProvidedPrice { get; }
    public decimal MinPrice { get; }
    public decimal MaxPrice { get; }

    public InvalidObjectPriceException(decimal price, decimal minPrice = 0.01m, decimal maxPrice = 1000000m)
        : base($"Object price must be between {minPrice:C} and {maxPrice:C}")
    {
        ProvidedPrice = price;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
    }
}