using MediatR;
using Microsoft.EntityFrameworkCore;
using MiBiblioteca.Application.Dtos;
using MiBiblioteca.Application.Exceptions;
using MiBiblioteca.Application.Interfaces;
using MiBiblioteca.Domain.Entities;

namespace MiBiblioteca.Application.Commands.Libros;

public class CrearLibroCommandHandler(IApplicationDbContext context)
    : IRequestHandler<CrearLibroCommand, LibroDto>
{
    public async Task<LibroDto> Handle(CrearLibroCommand request, CancellationToken cancellationToken)
    {
        // 0. Regla de negocio: no puede haber dos libros con el mismo ISBN-13.
        //    (El formato ya lo revisó el validador; acá revisamos contra la base.)
        if (!string.IsNullOrEmpty(request.Isbn13) &&
            await context.Libros.AnyAsync(l => l.Isbn_13 == request.Isbn13, cancellationToken))
        {
            throw new ConflictoException($"Ya existe un libro con el ISBN-13 {request.Isbn13}.");
        }

        // 1. Convertimos el pedido en una entidad del dominio.
        var libro = new Libro
        {
            Titulo = request.Titulo,
            Subtitulo = request.Subtitulo,
            Isbn_10 = request.Isbn10,
            Isbn_13 = request.Isbn13,
            Editorial = request.Editorial,
            AnioPublicacion = request.AnioPublicacion,
            PortadaUrl = request.PortadaUrl,
            Paginas = request.Paginas,
            CreatedAt = DateTime.UtcNow // PostgreSQL (timestamp with time zone) exige UTC
        };

        // 2. Add solo lo marca para insertar; SaveChangesAsync ejecuta el INSERT.
        context.Libros.Add(libro);
        await context.SaveChangesAsync(cancellationToken);

        // 3. Después de guardar, EF completa libro.LibroId con el ID que generó la base.
        return libro.ADto();
    }
}
