namespace MiBiblioteca.Application.Dtos;

// DTO (Data Transfer Object): es lo que la API devuelve al cliente.
// No devolvemos la entidad Libro directamente para controlar qué datos salen
// y para que un cambio en la base no rompa a quien consume la API.
public record LibroDto(
    int LibroId,
    string? Isbn10,
    string? Isbn13,
    string Titulo,
    string? Subtitulo,
    string? Editorial,
    string? AnioPublicacion,
    string? PortadaUrl,
    int Paginas,
    DateTime CreatedAt);
