using FluentValidation;

namespace MiBiblioteca.Application.Commands.Libros;

public class EliminarLibroCommandValidator : AbstractValidator<EliminarLibroCommand>
{
    public EliminarLibroCommandValidator()
    {
        RuleFor(x => x.LibroId)
            .GreaterThan(0).WithMessage("El id del libro debe ser mayor a 0.");
    }
}
