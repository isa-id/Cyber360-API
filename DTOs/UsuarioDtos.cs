namespace backend.DTOs
{
    // DTO para mostrar en la tabla
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Celular { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public bool Estado { get; set; }
    }

    // DTO para crear o editar usuario
    public class UsuarioCreateOrUpdateDto
    {
        public string TipoDoc { get; set; } = null!;
        public string Documento { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Celular { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public int FkRol { get; set; }
        public bool Estado { get; set; } = true;
        public string? Contrasena { get; set; } // opcional en edición
    }
}
