namespace MiBiblioteca.Application.Commands.Libros;

// Los datos editables de un libro. Lo implementan CrearLibroCommand y ActualizarLibroCommand
// para poder compartir las mismas reglas de validación (LibroDatosValidator).
public interface ILibroDatos
{
    string Titulo { get; }
    string? Subtitulo { get; }
    string? Isbn10 { get; }
    string? Isbn13 { get; }
    string? Editorial { get; }
    string? AnioPublicacion { get; }
    string? PortadaUrl { get; }
    int Paginas { get; }
}
