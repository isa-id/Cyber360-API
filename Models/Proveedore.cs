using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Proveedore
{
    public int Id { get; set; }

    public string? TipoPersona { get; set; }

    public string? NumeroDocumento { get; set; }

    public string? TipoDocumento { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? RazonSocial { get; set; }

    public string? Correo { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
