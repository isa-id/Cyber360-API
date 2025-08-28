using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Categoria
{
    public int Id { get; set; }

    public string? TipoCategoria { get; set; }

    public string? NombreCategoria { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
