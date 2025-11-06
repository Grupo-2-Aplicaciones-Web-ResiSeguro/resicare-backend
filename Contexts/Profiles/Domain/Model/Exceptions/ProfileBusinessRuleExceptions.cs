
namespace learning_center_webapi.Contexts.Profiles.Domain.Exceptions;

public abstract class ProfileBusinessRuleException : Exception
{
    protected ProfileBusinessRuleException(string message) : base(message) { }
}

public class ProfileNotFoundException : ProfileBusinessRuleException
{
    public ProfileNotFoundException(int id) 
        : base($"Profile with id {id} not found") { }
}

public class ProfileAlreadyExistsException : ProfileBusinessRuleException
{
    public ProfileAlreadyExistsException(int userId) 
        : base($"Profile for user {userId} already exists") { }
}

public class InvalidProfileDataException : ProfileBusinessRuleException
{
    public InvalidProfileDataException(string message) 
        : base(message) { }
}