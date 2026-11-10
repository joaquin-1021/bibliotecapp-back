using MediatR;

namespace MiBiblioteca.Application.Commands.Libros;

// IRequest sin <T>: el command no devuelve nada, solo se ejecuta.
public record EliminarLibroCommand(int LibroId) : IRequest;
