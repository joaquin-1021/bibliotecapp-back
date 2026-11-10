using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MiBiblioteca.Application.Behaviors;

namespace MiBiblioteca.Application;

// Cada capa registra sus propios servicios. Program.cs solo llama a AddApplication().
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg =>
        {
            // Busca en este proyecto todas las clases que implementan IRequestHandler
            // y las registra, así MediatR sabe qué handler resuelve cada Query o Command.
            cfg.RegisterServicesFromAssembly(assembly);

            // Todos los pedidos pasan por ValidationBehavior antes de llegar a su handler
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Registra todos los validadores (clases que heredan de AbstractValidator<T>)
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
