using FluentValidation;

namespace MiBiblioteca.Application.Queries.Libros;

// Las queries también pueden tener validador: ValidationBehavior lo ejecuta igual que con los commands.
public class ObtenerLibroPorIdQueryValidator : AbstractValidator<ObtenerLibroPorIdQuery>
{
    public ObtenerLibroPorIdQueryValidator()
    {
        RuleFor(x => x.LibroId)
            .GreaterThan(0).WithMessage("El id del libro debe ser mayor a 0.");
    }
}
