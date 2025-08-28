using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Abono
{
    public int Id { get; set; }

    public int? NumAbono { get; set; }

    public DateOnly? FechaAbono { get; set; }

    public decimal? PrecioPagar { get; set; }

    public decimal? Debe { get; set; }

    public decimal? ListaTotalAbonos { get; set; }

    public int? ReparacionId { get; set; }

    public virtual Reparacion? Reparacion { get; set; }
}
