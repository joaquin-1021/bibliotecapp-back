using MediatR;
using MiBiblioteca.Application.Dtos;

namespace MiBiblioteca.Application.Queries.Libros;

// Query: un pedido de LECTURA. No cambia nada en la base.
// Devuelve una página de libros, no todos: con miles de libros, traerlos
// de una vez sería lento para la base, la red y el frontend.
public record ObtenerLibrosQuery(int Pagina = 1, int TamanioPagina = 20)
    : IRequest<ResultadoPaginado<LibroDto>>;
