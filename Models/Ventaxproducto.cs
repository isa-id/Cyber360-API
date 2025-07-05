using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Ventaxproducto
{
    public int IdVentaxproducto { get; set; }

    public int FkVenta { get; set; }

    public int FkProducto { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Venta FkVentaNavigation { get; set; } = null!;
}
