namespace learning_center_webapi.Contexts.RegisteredObjects.Domain.Exceptions;

/// <summary>
/// Base exception for registered object business rule violations.
/// </summary>
public abstract class RegisteredObjectBusinessRuleException : Exception
{
    protected RegisteredObjectBusinessRuleException(string message) : base(message) { }
    protected RegisteredObjectBusinessRuleException(string message, Exception inner) : base(message, inner) { }
}