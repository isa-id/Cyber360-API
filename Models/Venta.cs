using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public DateOnly FechaVenta { get; set; }

    public int FkCliente { get; set; } 

    public string MetodoPago { get; set; } = null!;

    public decimal Total { get; set; }

    public string estado {get; set;} = null;

    public virtual Cliente? FkClienteNavigation { get; set; }

    public virtual ICollection<Servicioxventum> Servicioxventa { get; set; } = new List<Servicioxventum>();

    public virtual ICollection<Ventaxproducto> Ventaxproductos { get; set; } = new List<Ventaxproducto>();
}
