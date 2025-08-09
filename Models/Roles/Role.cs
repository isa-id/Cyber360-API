using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public string? Descripcion { get; set; } // Campo opcional (nullable)

    public bool Activo { get; set; } = true; // Valor por defecto true

    public virtual ICollection<Permisoxrol> Permisoxrols { get; set; } = new List<Permisoxrol>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
