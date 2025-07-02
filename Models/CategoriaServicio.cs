using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class CategoriaServicio
{
    public int IdCategoriaServicio { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
