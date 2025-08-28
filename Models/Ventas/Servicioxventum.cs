using System;
using System.Collections.Generic;

namespace backend.Models.Ventas;

public partial class Servicioxventum
{
    public int Id { get; set; }

    public int? ServicioId { get; set; }

    public string? Detalles { get; set; }

    public decimal? ValorTotal { get; set; }

    public int? VentaId { get; set; }

    public virtual Servicio? Servicio { get; set; }

    public virtual Venta? Venta { get; set; }
}
