using System;
using System.Collections.Generic;

namespace backend.Models.Ventas;

public partial class Reparacion
{
    public int Id { get; set; }

    public bool? Estado { get; set; }

    public bool? Prioridad { get; set; }

    public DateOnly? Fecha { get; set; }

    public DateOnly? FechaReparacion { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? DetallesDano { get; set; }

    public string? DetallesSolucion { get; set; }

    public bool? TipoReparacion { get; set; }

    public decimal? Valor { get; set; }

    public int? ClienteId { get; set; }

    public virtual ICollection<Abono> Abonos { get; set; } = new List<Abono>();

    public virtual Cliente? Cliente { get; set; }
}
