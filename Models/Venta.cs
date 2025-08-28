using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Venta
{
    public int Id { get; set; }

    public DateOnly? Fecha { get; set; }

    public int? ClienteId { get; set; }

    public decimal? Total { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<Productoxventum> Productoxventa { get; set; } = new List<Productoxventum>();

    public virtual ICollection<Servicioxventum> Servicioxventa { get; set; } = new List<Servicioxventum>();
}
