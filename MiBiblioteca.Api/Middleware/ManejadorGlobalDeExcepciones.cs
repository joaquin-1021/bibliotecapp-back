using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiBiblioteca.Application.Exceptions;

namespace MiBiblioteca.Api.Middleware;

// Atrapa cualquier excepción que no se haya manejado y la convierte en una respuesta
// HTTP clara, con el formato estándar ProblemDetails (RFC 9457).
// Así los handlers solo lanzan excepciones y no tienen que saber nada de HTTP.
public class ManejadorGlobalDeExcepciones(
    IProblemDetailsService problemDetailsService,
    ILogger<ManejadorGlobalDeExcepciones> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problema = exception switch
        {
            // 400: los datos enviados no cumplen las reglas del validador
            ValidationException validacion => new ValidationProblemDetails(
                validacion.Errors
                    // camelCase ("titulo" en vez de "Titulo") para que coincida con el JSON que envía el cliente
                    .GroupBy(e => JsonNamingPolicy.CamelCase.ConvertName(e.PropertyName))
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Los datos enviados no son válidos."
            },

            // 404: se pidió algo que no existe
            NoEncontradoException noEncontrado => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Recurso no encontrado.",
                Detail = noEncontrado.Message
            },

            // 409: el pedido choca con datos existentes
            ConflictoException conflicto => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflicto con datos existentes.",
                Detail = conflicto.Message
            },

            // 500: cualquier otro error. No mostramos el detalle al cliente
            // (puede tener información interna); queda en el log del servidor.
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Ocurrió un error inesperado."
            }
        };

        if (problema.Status == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Error no controlado");
        }

        httpContext.Response.StatusCode = problema.Status!.Value;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problema,
            Exception = exception
        });
    }
}
