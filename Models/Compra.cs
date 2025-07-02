using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    public int NumeroCompra { get; set; }

    public int FkProveedor { get; set; }

    public DateOnly FechaCompra { get; set; }

    public DateOnly FechaRegistro { get; set; }

    public decimal Total { get; set; }

    public virtual ICollection<ComprasXProducto> ComprasXProductos { get; set; } = new List<ComprasXProducto>();

    public virtual Proveedore FkProveedorNavigation { get; set; } = null!;
}
