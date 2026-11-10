using MediatR;
using Microsoft.EntityFrameworkCore;
using MiBiblioteca.Application.Exceptions;
using MiBiblioteca.Application.Interfaces;

namespace MiBiblioteca.Application.Commands.Libros;

public class EliminarLibroCommandHandler(IApplicationDbContext context)
    : IRequestHandler<EliminarLibroCommand>
{
    public async Task Handle(EliminarLibroCommand request, CancellationToken cancellationToken)
    {
        var libro = await context.Libros.FindAsync([request.LibroId], cancellationToken)
            ?? throw new NoEncontradoException($"No existe un libro con id {request.LibroId}.");

        // Regla de negocio: en la base, borrar un libro borra EN CASCADA sus ejemplares
        // y los préstamos de esos ejemplares. Para no perder historial sin querer,
        // solo se puede borrar un libro que no tenga ejemplares.
        if (await context.Ejemplares.AnyAsync(e => e.LibroId == request.LibroId, cancellationToken))
        {
            throw new ConflictoException("No se puede eliminar el libro porque tiene ejemplares registrados.");
        }

        // Remove lo marca para borrar; SaveChangesAsync ejecuta el DELETE.
        context.Libros.Remove(libro);
        await context.SaveChangesAsync(cancellationToken);
    }
}
