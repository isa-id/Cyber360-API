using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int FkRol { get; set; }

    public string TipoDoc { get; set; } = null!;

    public string Documento { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string Celular { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? CodigoRecuperacion { get; set; }

    public DateTime? CodigoExpira { get; set; }

    public bool Estado { get; set; }
    
        public virtual Role FkRolNavigation { get; set; } = null!;
}
