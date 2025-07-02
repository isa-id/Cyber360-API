using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class ComprasXProducto
{
    public int IdComprasXProductos { get; set; }

    public int FkProducto { get; set; }

    public int FkCompra { get; set; }

    public decimal PrecioUnit { get; set; }

    public int Cantidad { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Compra FkCompraNavigation { get; set; } = null!;

    public virtual Producto FkProductoNavigation { get; set; } = null!;
}
