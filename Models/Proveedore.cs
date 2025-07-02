using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Proveedore
{
    public int IdProveedor { get; set; }

    public string Documento { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public string NombreRa { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Contacto { get; set; } = null!;

    public bool Estado { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
