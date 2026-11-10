using FluentValidation;
using MediatR;

namespace MiBiblioteca.Application.Behaviors;

// Un "behavior" es un paso que MediatR ejecuta ANTES (y después) de cada handler,
// como un filtro por el que pasan todos los pedidos.
// Este busca los validadores del pedido; si hay errores, corta y lanza ValidationException,
// así ningún handler recibe datos inválidos y no hay que validar en cada uno.
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validadores)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Si el pedido no tiene validador (por ejemplo ObtenerLibrosQuery), sigue de largo
        if (!validadores.Any())
        {
            return await next(cancellationToken);
        }

        var contexto = new ValidationContext<TRequest>(request);
        var resultados = await Task.WhenAll(
            validadores.Select(v => v.ValidateAsync(contexto, cancellationToken)));

        var errores = resultados
            .SelectMany(r => r.Errors)
            .Where(e => e is not null)
            .ToList();

        if (errores.Count != 0)
        {
            throw new ValidationException(errores);
        }

        // Todo bien: pasa al siguiente paso (el handler)
        return await next(cancellationToken);
    }
}
