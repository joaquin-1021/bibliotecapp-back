using MediatR;
using MiBiblioteca.Application.Commands.Libros;
using MiBiblioteca.Application.Queries.Libros;

namespace MiBiblioteca.Api.Endpoints;

// La capa Api solo traduce HTTP <-> MediatR: no tiene lógica ni toca la base.
public static class LibrosEndpoints
{
    public static IEndpointRouteBuilder MapLibrosEndpoints(this IEndpointRouteBuilder app)
    {
        // Todas las rutas de este grupo empiezan con /libros
        var grupo = app.MapGroup("/libros").WithTags("Libros");

        // GET /libros?pagina=2&tamanioPagina=10 -> 200 OK con una página de libros.
        // Los parámetros con valor por defecto son opcionales: GET /libros = página 1 de 20.
        grupo.MapGet("/", async (ISender sender, CancellationToken ct, int pagina = 1, int tamanioPagina = 20) =>
        {
            var resultado = await sender.Send(new ObtenerLibrosQuery(pagina, tamanioPagina), ct);
            return TypedResults.Ok(resultado);
        });

        // GET /libros/5 -> 200 OK con el libro, o 404 si no existe.
        // {id:int} es una restricción de ruta: /libros/abc ni siquiera entra acá (da 404).
        grupo.MapGet("/{id:int}", async (int id, ISender sender, CancellationToken ct) =>
        {
            var libro = await sender.Send(new ObtenerLibroPorIdQuery(id), ct);
            return TypedResults.Ok(libro);
        });

        // POST /libros -> 201 Created con el libro creado.
        // El header Location apunta a GET /libros/{id}, donde se puede consultar.
        // El JSON del body se convierte automáticamente en CrearLibroCommand.
        grupo.MapPost("/", async (CrearLibroCommand command, ISender sender, CancellationToken ct) =>
        {
            var libro = await sender.Send(command, ct);
            return TypedResults.Created($"/libros/{libro.LibroId}", libro);
        });

        // PUT /libros/5 -> 200 OK con el libro actualizado, 404 si no existe.
        // El id sale de la URL (no del body): "with" crea una copia del command
        // con LibroId reemplazado, así un id distinto en el body no tiene efecto.
        grupo.MapPut("/{id:int}", async (int id, ActualizarLibroCommand command, ISender sender, CancellationToken ct) =>
        {
            var libro = await sender.Send(command with { LibroId = id }, ct);
            return TypedResults.Ok(libro);
        });

        // DELETE /libros/5 -> 204 No Content (se borró, no hay nada que devolver),
        // 404 si no existe, 409 si tiene ejemplares.
        grupo.MapDelete("/{id:int}", async (int id, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new EliminarLibroCommand(id), ct);
            return TypedResults.NoContent();
        });

        return app;
    }
}
