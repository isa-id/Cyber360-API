using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public string Documento { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string Celular { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public DateTime Tiempo { get; set; }

    public int Fichos { get; set; }

    public virtual ICollection<Reparacione> Reparaciones { get; set; } = new List<Reparacione>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
