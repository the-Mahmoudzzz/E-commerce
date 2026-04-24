using e_commerce.core.Exceptions;
using System.Net;
using System.Text.Json;

namespace e_commerce.api.Middleware
{
    /// <summary>
    /// Global exception handling middleware that catches all unhandled exceptions
    /// and returns standardized error responses
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
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
            context.Response.ContentType = "application/json";

            var (statusCode, errorCode, message, errors) = MapException(exception);

            context.Response.StatusCode = statusCode;

            // Log the error
            LogException(exception, statusCode, context);

            var response = new
            {
                Success = false,
                ErrorCode = errorCode,
                Message = message,
                Errors = errors,
                Timestamp = DateTime.UtcNow,
                RequestId = context.TraceIdentifier,
                // Include stack trace only in development
                StackTrace = _env.IsDevelopment() ? exception.StackTrace : null
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }

        private (int statusCode, string errorCode, string message, Dictionary<string, string[]>? errors) MapException(Exception exception)
        {
            return exception switch
            {
                // Domain exceptions
                NotFoundException ex => (ex.StatusCode, ex.ErrorCode, ex.Message, null),
                UnauthorizedException ex => (ex.StatusCode, ex.ErrorCode, ex.Message, null),
                AuthenticationException ex => (ex.StatusCode, ex.ErrorCode, ex.Message, null),
                ValidationException ex => (ex.StatusCode, ex.ErrorCode, ex.Message, ex.Errors),
                BusinessRuleException ex => (ex.StatusCode, ex.ErrorCode, ex.Message, null),
                ConflictException ex => (ex.StatusCode, ex.ErrorCode, ex.Message, null),
                DomainException ex => (ex.StatusCode, ex.ErrorCode, ex.Message, null),

                // Microsoft validation exception
                FluentValidation.ValidationException ex => (400, "VALIDATION_ERROR", "Validation failed",
                    ex.Errors.GroupBy(e => e.PropertyName)
                      .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())),

                // Identity exceptions
                UnauthorizedAccessException => (401, "UNAUTHORIZED", "Unauthorized access", null),

                // Database exceptions
                Microsoft.EntityFrameworkCore.DbUpdateException ex => HandleDbUpdateException(ex),

                // Default
                _ => (500, "INTERNAL_ERROR", "An unexpected error occurred. Please try again later.", null)
            };
        }

        private (int, string, string, Dictionary<string, string[]>?) HandleDbUpdateException(Microsoft.EntityFrameworkCore.DbUpdateException ex)
        {
            // Check for specific database errors
            var innerException = ex.InnerException?.Message?.ToLower() ?? "";

            if (innerException.Contains("unique") || innerException.Contains("duplicate"))
            {
                return (409, "DUPLICATE_KEY", "A record with this information already exists.", null);
            }

            if (innerException.Contains("foreign key") || innerException.Contains("constraint"))
            {
                return (400, "CONSTRAINT_VIOLATION", "This operation violates a database constraint.", null);
            }

            return (500, "DATABASE_ERROR", "A database error occurred.", null);
        }

        private void LogException(Exception exception, int statusCode, HttpContext context)
        {
            var path = context.Request.Path;
            var method = context.Request.Method;

            var logMessage = $"Error {statusCode} on {method} {path}: {exception.Message}";

            if (statusCode >= 500)
            {
                _logger.LogError(exception, logMessage);
            }
            else if (statusCode == 404)
            {
                _logger.LogWarning(logMessage);
            }
            else
            {
                _logger.LogInformation(logMessage);
            }
        }
    }
}
