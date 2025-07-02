using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Servicioxventum
{
    public int IdServicioxventa { get; set; }

    public int FkVenta { get; set; }

    public int FkServicio { get; set; }

    public int Cantidad { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Servicio FkServicioNavigation { get; set; } = null!;

    public virtual Venta FkVentaNavigation { get; set; } = null!;
}
