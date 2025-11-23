using System.Net;
using System.Text.Json;
using lentynaBackEnd.DTOs.Common;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

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
                _logger.LogError(ex, "Ivyko nenumatyta klaida");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = GetErrorResponse(exception);

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

        private static (HttpStatusCode statusCode, string message) GetErrorResponse(Exception exception)
        {
            // Handle DbUpdateException (database errors)
            if (exception is DbUpdateException dbEx)
            {
                return HandleDbUpdateException(dbEx);
            }

            return exception switch
            {
                ArgumentNullException => (HttpStatusCode.BadRequest, "Neteisingi uzklausos duomenys"),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Neautorizuota prieiga"),
                KeyNotFoundException => (HttpStatusCode.NotFound, "Resursas nerastas"),
                InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "Ivyko serverio klaida")
            };
        }

        private static (HttpStatusCode statusCode, string message) HandleDbUpdateException(DbUpdateException exception)
        {
            // Check for MySQL specific errors
            if (exception.InnerException is MySqlException mysqlEx)
            {
                return mysqlEx.Number switch
                {
                    // Duplicate entry error
                    1062 => (HttpStatusCode.Conflict, GetDuplicateKeyMessage(mysqlEx.Message)),
                    // Foreign key constraint error
                    1451 => (HttpStatusCode.Conflict, "Negalima istrinti - yra susijusiu irasu"),
                    1452 => (HttpStatusCode.BadRequest, "Nurodytas susijesis irasas neegzistuoja"),
                    // Other MySQL errors
                    _ => (HttpStatusCode.BadRequest, "Duomenu bazes klaida")
                };
            }

            return (HttpStatusCode.InternalServerError, "Ivyko duomenu bazes klaida");
        }

        private static string GetDuplicateKeyMessage(string errorMessage)
        {
            // Parse the MySQL error message to extract the field name
            // Example: "Duplicate entry '251548148184' for key 'IX_Knygos_ISBN'"
            if (errorMessage.Contains("IX_Knygos_ISBN"))
                return "Knyga su tokiu ISBN jau egzistuoja";
            if (errorMessage.Contains("IX_Naudotojai_el_pastas"))
                return "Naudotojas su tokiu el. pastu jau egzistuoja";
            if (errorMessage.Contains("IX_Naudotojai_slapyvardis"))
                return "Naudotojas su tokiu slapyvardziu jau egzistuoja";
            if (errorMessage.Contains("IX_Zanrai_pavadinimas"))
                return "Zanras su tokiu pavadinimu jau egzistuoja";

            return "Irasas su tokiais duomenimis jau egzistuoja";
        }
    }
}
