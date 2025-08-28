
using System.Text.Json.Serialization;
using backend.Models.Compras;
using System;
using System.Collections.Generic;


namespace backend.Models.Ventas;

public partial class Producto
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public DateOnly? FechaCreacion { get; set; }

    public int? CategoriaId { get; set; }

    public virtual Categoria? Categoria { get; set; }

    [JsonIgnore]
    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();
    [JsonIgnore]
    public virtual ICollection<Productoxventum> Productoxventa { get; set; } = new List<Productoxventum>();
}
