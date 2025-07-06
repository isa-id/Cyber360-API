using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Servicio
{
    public int IdServicio { get; set; }

    public string NombreServicio { get; set; } = null!;

    public decimal Precio { get; set; }

    public string Detalles { get; set; } = null!;

    public int FkCategoriaServicio { get; set; } = null;

    public int FkImagen { get; set; } = null;

    public int FkEquipo { get; set; } = null;

    public virtual CategoriaServicio FkCategoriaServicioNavigation { get; set; } = null;

    public virtual Equipo? FkEquipoNavigation { get; set; } 

    public virtual ImagenesServicio? FkImagenNavigation { get; set; } 

    public virtual ICollection<Servicioxinsumo> Servicioxinsumos { get; set; } = new List<Servicioxinsumo>();

    public virtual ICollection<Servicioxventum> Servicioxventa { get; set; } = new List<Servicioxventum>();
}
