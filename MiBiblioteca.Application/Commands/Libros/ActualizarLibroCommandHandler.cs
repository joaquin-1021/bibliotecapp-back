using MediatR;
using Microsoft.EntityFrameworkCore;
using MiBiblioteca.Application.Dtos;
using MiBiblioteca.Application.Exceptions;
using MiBiblioteca.Application.Interfaces;

namespace MiBiblioteca.Application.Commands.Libros;

public class ActualizarLibroCommandHandler(IApplicationDbContext context)
    : IRequestHandler<ActualizarLibroCommand, LibroDto>
{
    public async Task<LibroDto> Handle(ActualizarLibroCommand request, CancellationToken cancellationToken)
    {
        // 1. Buscamos el libro. FindAsync busca por clave primaria.
        //    EF queda "siguiendo" (tracking) esta entidad: detecta qué propiedades cambian.
        var libro = await context.Libros.FindAsync([request.LibroId], cancellationToken)
            ?? throw new NoEncontradoException($"No existe un libro con id {request.LibroId}.");

        // 2. Regla de negocio: el ISBN-13 no puede ser de OTRO libro
        //    (que siga siendo el mismo de este libro está bien).
        if (!string.IsNullOrEmpty(request.Isbn13) &&
            await context.Libros.AnyAsync(l => l.Isbn_13 == request.Isbn13 && l.LibroId != request.LibroId, cancellationToken))
        {
            throw new ConflictoException($"Ya existe otro libro con el ISBN-13 {request.Isbn13}.");
        }

        // 3. Cambiamos los datos. No hace falta llamar a Update(): como EF está siguiendo
        //    la entidad, SaveChangesAsync genera un UPDATE solo con las columnas que cambiaron.
        libro.Titulo = request.Titulo;
        libro.Subtitulo = request.Subtitulo;
        libro.Isbn_10 = request.Isbn10;
        libro.Isbn_13 = request.Isbn13;
        libro.Editorial = request.Editorial;
        libro.AnioPublicacion = request.AnioPublicacion;
        libro.PortadaUrl = request.PortadaUrl;
        libro.Paginas = request.Paginas;
        // CreatedAt no se toca: es la fecha de creación

        await context.SaveChangesAsync(cancellationToken);

        return libro.ADto();
    }
}
