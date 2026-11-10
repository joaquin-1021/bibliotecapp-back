using System.Linq.Expressions;
using MiBiblioteca.Domain.Entities;

namespace MiBiblioteca.Application.Dtos;

// Un único lugar donde se define cómo un Libro se convierte en LibroDto.
// Si mañana agregás un campo al DTO, lo cambiás solo acá (principio DRY: Don't Repeat Yourself).
public static class LibroMapeos
{
    // Es una Expression (no un método común) para que EF pueda traducirla a SQL
    // dentro de un .Select(...) y traer de la base solo las columnas necesarias.
    public static readonly Expression<Func<Libro, LibroDto>> Proyeccion = l => new LibroDto(
        l.LibroId,
        l.Isbn_10,
        l.Isbn_13,
        l.Titulo,
        l.Subtitulo,
        l.Editorial,
        l.AnioPublicacion,
        l.PortadaUrl,
        l.Paginas,
        l.CreatedAt);

    // La misma conversión compilada una sola vez, para usar con un Libro ya cargado en memoria.
    private static readonly Func<Libro, LibroDto> Convertir = Proyeccion.Compile();

    public static LibroDto ADto(this Libro libro) => Convertir(libro);
}
