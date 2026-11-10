namespace MiBiblioteca.Domain.Entities;

public class TipoUsuario
{
    public int TipoUsuarioId { get; set; }    
    public string Tipo { get; set; } = string.Empty;    
    public string Descripcion { get; set; } = string.Empty;
    //navegación
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}