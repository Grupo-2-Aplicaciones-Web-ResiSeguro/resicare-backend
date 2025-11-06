
namespace learning_center_webapi.Contexts.IAM.Domain.Exceptions;

public abstract class IAMBusinessRuleException : Exception
{
    protected IAMBusinessRuleException(string message) : base(message) { }
}

public class UserNotFoundException : IAMBusinessRuleException
{
    public UserNotFoundException(int id) 
        : base($"User with id {id} not found") { }
}

public class UserAlreadyExistsException : IAMBusinessRuleException
{
    public UserAlreadyExistsException(string email) 
        : base($"User with email {email} already exists") { }
}

public class InvalidCredentialsException : IAMBusinessRuleException
{
    public InvalidCredentialsException() 
        : base("Invalid email or password") { }
}

public class InvalidEmailFormatException : IAMBusinessRuleException
{
    public InvalidEmailFormatException(string email) 
        : base($"Invalid email format: {email}") { }
}

public class WeakPasswordException : IAMBusinessRuleException
{
    public WeakPasswordException() 
        : base("Password must be at least 8 characters") { }
}