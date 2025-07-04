public partial class Venta
{
    public int IdVenta { get; set; }

    public DateOnly FechaVenta { get; set; }

    public int FkCliente { get; set; } // Hacerlo nullable

    public string MetodoPago { get; set; } = null!;

    public decimal Total { get; set; }

    public virtual Cliente? FkClienteNavigation { get; set; } // Mantener nullable

    public virtual ICollection<Servicioxventum> Servicioxventa { get; set; } = new List<Servicioxventum>();

    public virtual ICollection<Ventaxproducto> Ventaxproductos { get; set; } = new List<Ventaxproducto>();
}
