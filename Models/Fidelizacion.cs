using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Fidelizacion
{
    public int Id { get; set; }

    public int? Horas { get; set; }

    public bool? TipoFicho { get; set; }

    public int? Fichos { get; set; }

    public int? FichosNa { get; set; }

    public bool? Estado { get; set; }

    public int? ClienteId { get; set; }

    public virtual Cliente? Cliente { get; set; }
}
