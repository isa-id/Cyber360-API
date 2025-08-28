using System.Text.Json.Serialization;
using backend.Models.Ventas;
using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Categoria
{
    public int Id { get; set; }

    public string? TipoCategoria { get; set; }

    public string? NombreCategoria { get; set; }

    public string? Descripcion { get; set; }

    [JsonIgnore]
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    [JsonIgnore]
    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}