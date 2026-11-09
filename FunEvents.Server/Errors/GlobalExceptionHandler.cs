using FunEvents.Application.Common.Exceptions;
using FunEvents.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace FunEvents.Server.Errors
{
    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (httpContext.Response.HasStarted)
            {
                return false;
            }

            var (statusCode, errorCode, userMessage) = exception switch
            {
                NotFoundException ex =>
                    (StatusCodes.Status404NotFound, "not_found", ex.Message),

                DomainException ex =>
                    (StatusCodes.Status400BadRequest, "validation_error", ex.Message),

                ArgumentException ex =>
                    (StatusCodes.Status400BadRequest, "validation_error", ex.Message),

                DbUpdateConcurrencyException =>
                    (StatusCodes.Status409Conflict, "concurrency_conflict", "El inventario cambió mientras procesábamos tu solicitud. Por favor, inténtalo de nuevo."),

                DbUpdateException =>
                    (StatusCodes.Status409Conflict, "integrity_conflict", "No se pudieron guardar los cambios debido a una restricción de datos."),

                _ =>
                    (StatusCodes.Status500InternalServerError, "internal_error", "Ha ocurrido un error interno inesperado en nuestro servidor.")
            };

            if (statusCode >= StatusCodes.Status500InternalServerError)
            {
                logger.LogError(exception, "Error crítico no controlado en {Path}", httpContext.Request.Path);
            }
            else
            {
                logger.LogWarning("Solicitud rechazada ({StatusCode}) en {Path}. Razón: {Message}",
                    statusCode, httpContext.Request.Path, exception.Message);
            }

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                code = errorCode,
                message = userMessage
            }, cancellationToken);

            return true;
        }
    }
}
