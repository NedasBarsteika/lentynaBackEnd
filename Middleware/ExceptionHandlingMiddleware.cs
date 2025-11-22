using System.Net;
using System.Text.Json;
using lentynaBackEnd.DTOs.Common;

namespace lentynaBackEnd.Middleware
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
                _logger.LogError(ex, "Įvyko nenumatyta klaida");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                ArgumentNullException => (HttpStatusCode.BadRequest, "Neteisingi užklausos duomenys"),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Neautorizuota prieiga"),
                KeyNotFoundException => (HttpStatusCode.NotFound, "Resursas nerastas"),
                InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "Įvyko serverio klaida")
            };

            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new ErrorResponseDto(
                (int)statusCode,
                message
            );

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
        }
    }
}
