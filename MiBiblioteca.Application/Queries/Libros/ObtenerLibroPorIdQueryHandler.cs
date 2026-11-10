using MediatR;
using Microsoft.EntityFrameworkCore;
using MiBiblioteca.Application.Dtos;
using MiBiblioteca.Application.Exceptions;
using MiBiblioteca.Application.Interfaces;

namespace MiBiblioteca.Application.Queries.Libros;

public class ObtenerLibroPorIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<ObtenerLibroPorIdQuery, LibroDto>
{
    public async Task<LibroDto> Handle(ObtenerLibroPorIdQuery request, CancellationToken cancellationToken)
    {
        // Where + Select + FirstOrDefaultAsync: un solo SELECT ... WHERE "LibroId" = @id LIMIT 1
        // Devuelve null si no hay ninguno (por eso LibroDto?).
        LibroDto? libro = await context.Libros
            .Where(l => l.LibroId == request.LibroId)
            .Select(LibroMapeos.Proyeccion)
            .FirstOrDefaultAsync(cancellationToken);

        // No existe -> excepción con significado; la Api la convierte en 404
        return libro ?? throw new NoEncontradoException($"No existe un libro con id {request.LibroId}.");
    }
}
