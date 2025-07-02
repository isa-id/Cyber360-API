using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Servicioxinsumo
{
    public int IdServicioxinsumo { get; set; }

    public int FkProducto { get; set; }

    public int FkServicio { get; set; }

    public virtual Producto FkProductoNavigation { get; set; } = null!;

    public virtual Servicio FkServicioNavigation { get; set; } = null!;
}
