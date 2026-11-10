using MediatR;
using MiBiblioteca.Application.Dtos;

namespace MiBiblioteca.Application.Commands.Libros;

// Command: un pedido de ESCRITURA. Lleva los datos que manda el cliente
// en el body del POST y devuelve el libro ya creado (con su LibroId).
public record CrearLibroCommand(
    string Titulo,
    string? Subtitulo,
    string? Isbn10,
    string? Isbn13,
    string? Editorial,
    string? AnioPublicacion,
    string? PortadaUrl,
    int Paginas) : IRequest<LibroDto>, ILibroDatos;
