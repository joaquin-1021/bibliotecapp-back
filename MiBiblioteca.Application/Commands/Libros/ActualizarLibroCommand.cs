using MediatR;
using MiBiblioteca.Application.Dtos;

namespace MiBiblioteca.Application.Commands.Libros;

// Command para editar un libro existente. PUT reemplaza TODOS los datos editables:
// si un campo opcional no se envía, queda en null.
// LibroId no viene en el body: lo completa el endpoint con el {id} de la URL.
public record ActualizarLibroCommand(
    int LibroId,
    string Titulo,
    string? Subtitulo,
    string? Isbn10,
    string? Isbn13,
    string? Editorial,
    string? AnioPublicacion,
    string? PortadaUrl,
    int Paginas) : IRequest<LibroDto>, ILibroDatos;
