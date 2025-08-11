namespace Cyber360.DTOs
{
    public class RoleCreateDto
{
    public int? IdRol { get; set; } // Nullable para POST donde no hay ID aún
    public string NombreRol { get; set; } = null!;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;
    public List<int> PermisosIds { get; set; } = new List<int>();
}

}