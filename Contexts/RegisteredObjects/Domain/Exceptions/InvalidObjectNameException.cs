namespace learning_center_webapi.Contexts.RegisteredObjects.Domain.Exceptions;

/// <summary>
/// Exception thrown when the object name is invalid.
/// </summary>
public sealed class InvalidObjectNameException : RegisteredObjectBusinessRuleException
{
    public string ProvidedName { get; }
    public int MinLength { get; }
    public int MaxLength { get; }

    public InvalidObjectNameException(string name, int minLength = 3, int maxLength = 100)
        : base($"Object name must be between {minLength} and {maxLength} characters")
    {
        ProvidedName = name;
        MinLength = minLength;
        MaxLength = maxLength;
    }
}