using System;
using System.Collections.Generic;

namespace backend.Models.Ventas;

public partial class Servicio
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public decimal? Precio { get; set; }

    public string? Detalles { get; set; }

    public int? CategoriaId { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual ICollection<Servicioxventum> Servicioxventa { get; set; } = new List<Servicioxventum>();
}
