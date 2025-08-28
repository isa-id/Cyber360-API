using System;
using System.Collections.Generic;

namespace backend.Models.Compras;

public partial class Compra
{
    public int Id { get; set; }

    public int? ProveedorId { get; set; }

    public DateOnly? FechaCompra { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public string? MetodoPago { get; set; }

    public string? Estado { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Iva { get; set; }

    public decimal? Total { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual Proveedore? Proveedor { get; set; }
}
