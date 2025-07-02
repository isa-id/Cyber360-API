using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Reparacione
{
    public int IdReparacion { get; set; }

    public int FkCliente { get; set; }

    public DateOnly Fecha { get; set; }

    public DateOnly FechaReparacion { get; set; }

    public DateOnly FechaEstimada { get; set; }

    public string DetallesDano { get; set; } = null!;

    public string DetallesSolucion { get; set; } = null!;

    public string TipoReparacion { get; set; } = null!;

    public int FkEstado { get; set; }

    public decimal Valor { get; set; }

    public decimal Adelanto { get; set; }

    public decimal ValorTotal { get; set; }

    public virtual Cliente FkClienteNavigation { get; set; } = null!;

    public virtual Estado FkEstadoNavigation { get; set; } = null!;
}
