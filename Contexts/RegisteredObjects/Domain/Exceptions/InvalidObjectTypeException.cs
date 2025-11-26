namespace learning_center_webapi.Contexts.RegisteredObjects.Domain.Exceptions;

/// <summary>
/// Exception thrown when an invalid object type is provided.
/// </summary>
public sealed class InvalidObjectTypeException : RegisteredObjectBusinessRuleException
{
    public string ProvidedType { get; }
    public string[] AllowedTypes { get; }

    public InvalidObjectTypeException(string type, string[] allowedTypes)
        : base($"Invalid object type '{type}'. Valid types are: {string.Join(", ", allowedTypes)}")
    {
        ProvidedType = type;
        AllowedTypes = allowedTypes;
    }
}