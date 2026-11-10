namespace MiBiblioteca.Domain.Entities;

public class Ejemplar
{
    public int EjemplarId { get; set; }
    public int LibroId { get; set; }
    public int UsuarioId { get; set; } // Dueño de la copia física
    
    public string Estado { get; set; } = string.Empty; // Ej: "Excelente", "Gastado"
    public string? Notas { get; set; }
    public DateTime CreatedAt { get; set; }

    //navegación
    public Libro LibroDetalle { get; set; } = null!;
    public Usuario Propietario { get; set; } = null!;
    public ICollection<Prestamo> HistorialPrestamos { get; set; } = new List<Prestamo>();
}