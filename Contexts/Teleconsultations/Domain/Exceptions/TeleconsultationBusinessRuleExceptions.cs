using System;

namespace learning_center_webapi.Contexts.Teleconsultations.Domain.Exceptions;


public abstract class TeleconsultationBusinessRuleException : Exception
{
    protected TeleconsultationBusinessRuleException(string message) : base(message) { }
    protected TeleconsultationBusinessRuleException(string message, Exception inner) : base(message, inner) { }
}

public sealed class TeleconsultationNotFoundException : TeleconsultationBusinessRuleException
{
    public TeleconsultationNotFoundException(string id)
        : base($"Teleconsultation with id '{id}' was not found.") { }
}

public sealed class TeleconsultationAlreadyCancelledException : TeleconsultationBusinessRuleException
{
    public TeleconsultationAlreadyCancelledException(string id)
        : base($"Teleconsultation with id '{id}' is already cancelled.") { }
}

public sealed class TeleconsultationConflictException : TeleconsultationBusinessRuleException
{
    public TeleconsultationConflictException(string message)
        : base(message) { }

    public TeleconsultationConflictException(string message, Exception inner)
        : base(message, inner) { }
}

public sealed class InvalidServiceException : TeleconsultationBusinessRuleException
{
    public InvalidServiceException(string service)
        : base($"Service '{service}' is not valid for teleconsultations.") { }
}

public sealed class UserNotAssociatedException : TeleconsultationBusinessRuleException
{
    public UserNotAssociatedException(string userId, string teleconsultationId)
        : base($"User '{userId}' is not associated with teleconsultation '{teleconsultationId}'.") { }
}