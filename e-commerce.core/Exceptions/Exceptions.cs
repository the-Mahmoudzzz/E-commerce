using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.Exceptions
{
    public class DomainException : Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }

        public DomainException(string message, string errorCode = "DOMAIN_ERROR", int statusCode = 400)
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }

    /// <summary>
    /// Thrown when a requested resource is not found
    /// </summary>
    public class NotFoundException : DomainException
    {
        public NotFoundException(string resourceName, object key)
            : base($"{resourceName} with key '{key}' was not found.", "NOT_FOUND", 404)
        {
        }
    }

    /// <summary>
    /// Thrown when user is not authorized to perform an action
    /// </summary>
    public class UnauthorizedException : DomainException
    {
        public UnauthorizedException(string message = "You are not authorized to perform this action.")
            : base(message, "UNAUTHORIZED", 403)
        {
        }
    }

    /// <summary>
    /// Thrown when authentication fails
    /// </summary>
    public class AuthenticationException : DomainException
    {
        public AuthenticationException(string message = "Authentication failed.")
            : base(message, "AUTHENTICATION_FAILED", 401)
        {
        }
    }

    /// <summary>
    /// Thrown when validation fails
    /// </summary>
    public class ValidationException : DomainException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException(string message, Dictionary<string, string[]> errors)
            : base(message, "VALIDATION_ERROR", 400)
        {
            Errors = errors ?? new Dictionary<string, string[]>();
        }

        public ValidationException(string field, string error)
            : base("Validation failed.", "VALIDATION_ERROR", 400)
        {
            Errors = new Dictionary<string, string[]> { { field, new[] { error } } };
        }
    }

    /// <summary>
    /// Thrown when a business rule is violated
    /// </summary>
    public class BusinessRuleException : DomainException
    {
        public BusinessRuleException(string message)
            : base(message, "BUSINESS_RULE_VIOLATION", 400)
        {
        }
    }

    /// <summary>
    /// Thrown when there's a conflict (e.g., duplicate data)
    /// </summary>
    public class ConflictException : DomainException
    {
        public ConflictException(string message)
            : base(message, "CONFLICT", 409)
        {
        }
    }
}

