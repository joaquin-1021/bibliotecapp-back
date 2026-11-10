using Microsoft.EntityFrameworkCore;

namespace MiBiblioteca.Application.Dtos;

// Respuesta paginada genérica: sirve para libros, usuarios, préstamos, etc.
// Además de los elementos de la página, informa los totales para que el frontend
// pueda dibujar el paginador ("Página 2 de 5").
public record ResultadoPaginado<T>(
    IReadOnlyList<T> Items,
    int Pagina,
    int TamanioPagina,
    int TotalItems)
{
    // Propiedades calculadas: salen en el JSON igual que las demás
    public int TotalPaginas => (int)Math.Ceiling(TotalItems / (double)TamanioPagina);
    public bool TienePaginaAnterior => Pagina > 1;
    public bool TienePaginaSiguiente => Pagina < TotalPaginas;

    // Ejecuta DOS consultas sobre la misma IQueryable:
    //   1. SELECT COUNT(*)                 -> total de elementos
    //   2. SELECT ... OFFSET x LIMIT y     -> solo los de esta página
    // La consulta debe venir ordenada: sin ORDER BY, la base no garantiza
    // el orden y un mismo elemento podría aparecer en dos páginas.
    public static async Task<ResultadoPaginado<T>> CrearAsync(
        IQueryable<T> consulta, int pagina, int tamanioPagina, CancellationToken cancellationToken)
    {
        var total = await consulta.CountAsync(cancellationToken);

        var items = await consulta
            .Skip((pagina - 1) * tamanioPagina)   // saltea las páginas anteriores (OFFSET)
            .Take(tamanioPagina)                  // toma solo esta página (LIMIT)
            .ToListAsync(cancellationToken);

        return new ResultadoPaginado<T>(items, pagina, tamanioPagina, total);
    }
}
