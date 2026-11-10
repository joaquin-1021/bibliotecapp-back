namespace MiBiblioteca.Domain.Entities;

public class Prestamo
{
    public int PrestamoId { get; set; }
    public int EjemplarId { get; set; }
    public int ContactoId { get; set; }

    public DateTime FechaPrestamo { get; set; }
    public DateTime? FechaDevolucion { get; set; }
    public string? Notas { get; set; }

    //navegacion
    public Ejemplar EjemplarPrestado { get; set; } = null!;
    public Contacto ContactoReceptor { get; set; } = null!;
} 