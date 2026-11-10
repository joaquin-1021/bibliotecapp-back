namespace MiBiblioteca.Domain.Entities;

public class Contacto
{
    public int ContactoId { get; set; }
    public int UsuarioId { get; set; } // El dueño de este contacto
    public int? UsuarioVinculadoId { get; set; } // Null si no tiene cuenta en la app
    
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }

    //navegación
    public Usuario UsuarioPropietario { get; set; } = null!;
    public Usuario? UsuarioVinculado { get; set; } 
    public ICollection<Prestamo> PrestamosRecibidos { get; set; } = new List<Prestamo>();
}