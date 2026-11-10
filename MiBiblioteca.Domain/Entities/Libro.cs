namespace MiBiblioteca.Domain.Entities;

public class Libro {
    public int LibroId { get; set; }
    public string? Isbn_10 { get; set; }
    public string? Isbn_13 { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Subtitulo { get; set; }
    public string? Editorial { get; set; }
    public string? AnioPublicacion { get; set; }
    public string? PortadaUrl { get; set; }
    public int Paginas { get; set; }
    public DateTime CreatedAt { get; set; }

    //navegacion
}