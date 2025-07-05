using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }

    public DateOnly FechaCreacion { get; set; }

    public int FkImagen { get; set; }

    public int FkCategoria { get; set; }

    public virtual ICollection<ComprasXProducto> ComprasXProductos { get; set; } = new List<ComprasXProducto>();
    
     public virtual CategoriaProducto FkCategoriaNavigation { get; set; }
    
    public virtual ImagenesProducto FkImagenNavigation { get; set; }

    public virtual ICollection<Servicioxinsumo> Servicioxinsumos { get; set; } = new List<Servicioxinsumo>();
}
