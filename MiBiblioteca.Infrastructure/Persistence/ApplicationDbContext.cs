using Microsoft.EntityFrameworkCore;
using MiBiblioteca.Application.Interfaces;
using MiBiblioteca.Domain.Entities;

namespace MiBiblioteca.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TipoUsuario> TiposUsuario => Set<TipoUsuario>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Contacto> Contactos => Set<Contacto>();
    public DbSet<Libro> Libros => Set<Libro>();
    public DbSet<Ejemplar> Ejemplares => Set<Ejemplar>();
    public DbSet<Prestamo> Prestamos => Set<Prestamo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aquí se usa para configurar nombres de tablas o relaciones complejas.
        // Por ejemplo, para forzar que la tabla se llame "tipo_usuario" en minúsculas:
        modelBuilder.Entity<TipoUsuario>().ToTable("tipo_usuario");
        modelBuilder.Entity<Usuario>().ToTable("usuario");
        modelBuilder.Entity<Contacto>().ToTable("contacto");
        modelBuilder.Entity<Libro>().ToTable("libro");
        modelBuilder.Entity<Ejemplar>().ToTable("ejemplar");
        modelBuilder.Entity<Prestamo>().ToTable("prestamo");
        
        // EF Core detecta la mayoría de las relaciones automáticamente gracias a cómo escribimos las propiedades de navegación en tus entidades.

        modelBuilder.Entity<Contacto>()
            .HasOne(c => c.UsuarioPropietario)
            .WithMany(u => u.ContactosAgenda)
            .HasForeignKey( c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Contacto>()
            .HasOne(c => c.UsuarioVinculado)
            .WithMany()
            .HasForeignKey(c => c.UsuarioVinculadoId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.TipoUsuario)
            .WithMany(t => t.Usuarios)
            .HasForeignKey(u => u.TipoUsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Datos iniciales: los tipos de usuario que ya tenías cargados en la base
        modelBuilder.Entity<TipoUsuario>().HasData(
            new TipoUsuario { TipoUsuarioId = 1, Tipo = "admin", Descripcion = "Administrador de la aplicación" },
            new TipoUsuario { TipoUsuarioId = 2, Tipo = "usuario", Descripcion = "Usuario estándar" }
        );
            
        // Duplicados: no permitimos dos usuarios con el mismo email
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Duplicados: no permitimos dos libros con el mismo ISBN-13
        modelBuilder.Entity<Libro>()
            .HasIndex(l => l.Isbn_13)
            .IsUnique();
    }
}