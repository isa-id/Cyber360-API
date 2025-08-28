using backend.Models.Ventas;
using System;
using System.Collections.Generic;

namespace backend.Models.Compras;

public partial class DetalleCompra
{
    public int Id { get; set; }

    public int? CompraId { get; set; }

    public int? ProductoId { get; set; }

    public int? Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? SubtotalItems { get; set; }

    public virtual Compra? Compra { get; set; }

    public virtual Producto? Producto { get; set; }
}
