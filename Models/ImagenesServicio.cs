using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class ImagenesServicio
{
    public int IdImagen { get; set; }

    public string Url { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
