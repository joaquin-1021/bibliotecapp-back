using MediatR;
using MiBiblioteca.Application.Dtos;

namespace MiBiblioteca.Application.Queries.Libros;

// Query de lectura de UN libro. Devuelve LibroDto (no una lista).
public record ObtenerLibroPorIdQuery(int LibroId) : IRequest<LibroDto>;
