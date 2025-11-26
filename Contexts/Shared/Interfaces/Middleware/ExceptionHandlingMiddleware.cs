// Contexts/Shared/Interfaces/Middleware/ExceptionHandlingMiddleware.cs
using System.Net;
using System.Text.Json;
using learning_center_webapi.Contexts.Claims.Domain.Exceptions;
using learning_center_webapi.Contexts.RegisteredObjects.Domain.Exceptions;
using learning_center_webapi.Contexts.Reminders.Domain.Exceptions;
using learning_center_webapi.Contexts.Teleconsultations.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace learning_center_webapi.Contexts.Shared.Interfaces.Middleware;

/// <summary>
/// Middleware for centralized exception handling and error response formatting.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = exception switch
        {
            // ============================================
            // Claims Domain Exceptions
            // ============================================
            DuplicateActiveClaimException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                Message = ex.Message,
                ErrorCode = "DUPLICATE_ACTIVE_CLAIM",
                Details = new
                {
                    ex.RegisteredObjectId,
                    ex.ExistingClaimId
                }
            },

            InvalidClaimStatusTransitionException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "INVALID_STATUS_TRANSITION",
                Details = new
                {
                    ex.CurrentStatus,
                    ex.NewStatus
                }
            },

            ClaimOutsideReportingPeriodException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "CLAIM_OUTSIDE_REPORTING_PERIOD",
                Details = new
                {
                    IncidentDate = ex.IncidentDate.ToString("yyyy-MM-dd"),
                    ex.MaxDaysAllowed
                }
            },

            InvalidRatingException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "INVALID_RATING",
                Details = new
                {
                    ex.ProvidedRating,
                    ex.MinRating,
                    ex.MaxRating
                }
            },

            ClaimNotFoundException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = ex.Message,
                ErrorCode = "CLAIM_NOT_FOUND",
                Details = new
                {
                    ex.ClaimId
                }
            },

            
            // ============================================
            // Teleconsultations Domain Exceptions
            // ============================================
            TeleconsultationTimeSlotConflictException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                Message = ex.Message,
                ErrorCode = "TELECONSULTATION_TIME_SLOT_CONFLICT",
                Details = new
                {
                    ex.Date,
                    ex.Time,
                    ex.ExistingTeleconsultationId
                }
            },

            TeleconsultationServiceSlotLimitException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                Message = ex.Message,
                ErrorCode = "TELECONSULTATION_SERVICE_SLOT_LIMIT",
                Details = new
                {
                    ex.Date,
                    ex.Time,
                    ex.Service,
                    ex.CurrentCount,
                    ex.MaxAllowed
                }
            },

            InvalidServiceTypeException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "INVALID_SERVICE_TYPE",
                Details = new
                {
                    ex.ProvidedService,
                    ex.AllowedServices
                }
            },

            TeleconsultationUserLimitException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "TELECONSULTATION_USER_LIMIT_EXCEEDED",
                Details = new
                {
                    ex.UserId,
                    ex.CurrentCount,
                    ex.MaxAllowed
                }
            },

            InvalidAppointmentDateException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "INVALID_APPOINTMENT_DATE",
                Details = null
            },

            TeleconsultationNotFoundException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = ex.Message,
                ErrorCode = "TELECONSULTATION_NOT_FOUND",
                Details = null
            },

            // ============================================
            // Reminders Domain Exceptions
            // ============================================
            ReminderNotFoundException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = ex.Message,
                ErrorCode = "REMINDER_NOT_FOUND",
                Details = new
                {
                    ex.ReminderId
                }
            },

            ReminderInPastException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "REMINDER_IN_PAST",
                Details = new
                {
                    AttemptedDateTime = ex.AttemptedDateTime.ToString("yyyy-MM-dd HH:mm")
                }
            },

            InvalidReminderTitleException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "INVALID_REMINDER_TITLE",
                Details = new
                {
                    ex.ProvidedTitle,
                    ex.MinLength,
                    ex.MaxLength
                }
            },

            InvalidReminderDateFormatException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "INVALID_REMINDER_DATE_FORMAT",
                Details = new
                {
                    ex.ProvidedDate,
                    ex.ExpectedFormat
                }
            },

            InvalidReminderTimeFormatException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "INVALID_REMINDER_TIME_FORMAT",
                Details = new
                {
                    ex.ProvidedTime,
                    ex.ExpectedFormat
                }
            },

            DuplicateReminderException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                Message = ex.Message,
                ErrorCode = "DUPLICATE_REMINDER",
                Details = new
                {
                    ex.UserId,
                    ex.Title,
                    ex.Date,
                    ex.Time,
                    ex.ExistingReminderId
                }
            },

            ReminderTooFarInFutureException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "REMINDER_TOO_FAR_IN_FUTURE",
                Details = new
                {
                    AttemptedDate = ex.AttemptedDate.ToString("yyyy-MM-dd"),
                    ex.MaxDaysAllowed
                }
            },

            // ============================================
            // RegisteredObjects Domain Exceptions
            // ============================================
            learning_center_webapi.Contexts.RegisteredObjects.Domain.Exceptions.RegisteredObjectNotFoundException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = ex.Message,
                ErrorCode = "REGISTERED_OBJECT_NOT_FOUND",
                Details = new
                {
                    ex.ObjectId
                }
            },

            InvalidObjectNameException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "INVALID_OBJECT_NAME",
                Details = new
                {
                    ex.ProvidedName,
                    ex.MinLength,
                    ex.MaxLength
                }
            },

            InvalidObjectTypeException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "INVALID_OBJECT_TYPE",
                Details = new
                {
                    ex.ProvidedType,
                    ex.AllowedTypes
                }
            },

            InvalidObjectPriceException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "INVALID_OBJECT_PRICE",
                Details = new
                {
                    ex.ProvidedPrice,
                    MinPrice = ex.MinPrice.ToString("C"),
                    MaxPrice = ex.MaxPrice.ToString("C")
                }
            },

            DuplicateSerialNumberException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                Message = ex.Message,
                ErrorCode = "DUPLICATE_SERIAL_NUMBER",
                Details = new
                {
                    ex.SerialNumber,
                    ex.UserId,
                    ex.ExistingObjectId
                }
            },

            CannotModifyObjectWithActiveClaimException ex => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message,
                ErrorCode = "CANNOT_MODIFY_OBJECT_WITH_ACTIVE_CLAIM",
                Details = new
                {
                    ex.ObjectId,
                    ex.ActiveClaimId
                }
            },

            // ============================================
            // Generic exceptions
            // ============================================
            _ => new ErrorResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "An internal server error occurred",
                ErrorCode = "INTERNAL_SERVER_ERROR"
            }
        };

        response.StatusCode = errorResponse.StatusCode;
        await response.WriteAsync(JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }

    private class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public object? Details { get; set; }
    }
}