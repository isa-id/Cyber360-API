using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class ImagenesProducto
{
    public int IdImagen { get; set; }

    public string Url { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
