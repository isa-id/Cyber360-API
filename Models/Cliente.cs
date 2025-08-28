using backend.Models.Ventas;
using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? TipoDoc { get; set; }

    public int? Documento { get; set; }

    public string? Correo { get; set; }

    public bool? Estado { get; set; }

    public virtual Fidelizacion? Fidelizacion { get; set; }

    public virtual ICollection<Reparacion> Reparacions { get; set; } = new List<Reparacion>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
