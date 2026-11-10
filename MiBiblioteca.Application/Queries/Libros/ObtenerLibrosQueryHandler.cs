using MediatR;
using MiBiblioteca.Application.Dtos;
using MiBiblioteca.Application.Interfaces;

namespace MiBiblioteca.Application.Queries.Libros;

// Handler: la clase que RESUELVE el pedido. MediatR la encuentra sola
// porque implementa IRequestHandler<ObtenerLibrosQuery, ...>.
// Recibe IApplicationDbContext (la interfaz), no ApplicationDbContext:
// Application no sabe que existe PostgreSQL, eso es tarea de Infrastructure.
public class ObtenerLibrosQueryHandler(IApplicationDbContext context)
    : IRequestHandler<ObtenerLibrosQuery, ResultadoPaginado<LibroDto>>
{
    public async Task<ResultadoPaginado<LibroDto>> Handle(ObtenerLibrosQuery request, CancellationToken cancellationToken)
    {
        // Esto todavía NO ejecuta nada: arma la consulta (IQueryable).
        // Se ejecuta recién dentro de ResultadoPaginado.CrearAsync.
        var consulta = context.Libros
            .OrderBy(l => l.Titulo)
            .ThenBy(l => l.LibroId)            // desempate: dos libros con el mismo título siempre en el mismo orden
            .Select(LibroMapeos.Proyeccion);   // EF solo trae las columnas del DTO

        return await ResultadoPaginado<LibroDto>.CrearAsync(
            consulta, request.Pagina, request.TamanioPagina, cancellationToken);
    }
}
