using Microsoft.EntityFrameworkCore;
using MiBiblioteca.Domain.Entities;

namespace MiBiblioteca.Application.Interfaces;

public interface IApplicationDbContext {
    DbSet<TipoUsuario> TiposUsuario { get; }
    DbSet<Usuario> Usuarios { get; }
    DbSet<Contacto> Contactos { get; }
    DbSet<Libro> Libros { get; }
    DbSet<Ejemplar> Ejemplares { get; }
    DbSet<Prestamo> Prestamos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}