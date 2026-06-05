using InventoryAPI.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace InventoryAPI.Middleware
{
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
            var (statusCode, message) = MapException(exception);

            if (statusCode == (int)HttpStatusCode.InternalServerError)
                _logger.LogError(exception, "Unhandled exception.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new { error = message };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static (int statusCode, string message) MapException(Exception exception)
        {
            return exception switch
            {
                NotFoundException ex => ((int)HttpStatusCode.NotFound, ex.Message),
                ConflictException ex => ((int)HttpStatusCode.Conflict, ex.Message),
                BusinessValidationException ex => (422, ex.Message),
                OperationCanceledException => ((int)HttpStatusCode.BadRequest, "La solicitud fue cancelada."),
                _ => ((int)HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.")
            };
        }
    }
}
