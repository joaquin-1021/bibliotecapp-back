using FluentValidation;

namespace MiBiblioteca.Application.Queries.Libros;

public class ObtenerLibrosQueryValidator : AbstractValidator<ObtenerLibrosQuery>
{
    // Límite para que nadie pida ?tamanioPagina=1000000 y sature la base
    public const int TamanioMaximo = 100;

    public ObtenerLibrosQueryValidator()
    {
        RuleFor(x => x.Pagina)
            .GreaterThanOrEqualTo(1).WithMessage("La página debe ser 1 o mayor.");

        RuleFor(x => x.TamanioPagina)
            .InclusiveBetween(1, TamanioMaximo)
            .WithMessage($"El tamaño de página debe estar entre 1 y {TamanioMaximo}.");
    }
}
