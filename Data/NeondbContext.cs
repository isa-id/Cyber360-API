using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Models.Compras;
using backend.Models.Ventas;

namespace backend.Data;

public partial class NeondbContext : DbContext
{
    public NeondbContext()
    {
    }

    public NeondbContext(DbContextOptions<NeondbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Abono> Abonos { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }

    public virtual DbSet<Fidelizacion> Fidelizacions { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Permisoxrol> Permisoxrols { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Productoxventum> Productoxventa { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Reparacion> Reparacions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<Servicioxventum> Servicioxventa { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=ep-twilight-leaf-a81vdkl5-pooler.eastus2.azure.neon.tech;Port=5432;Database=neondb;Username=neondb_owner;Password=npg_DId51CeRfMQg;SSL Mode=Require;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Abono>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("abonos_pkey");

            entity.ToTable("abonos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Debe)
                .HasPrecision(10, 2)
                .HasColumnName("debe");
            entity.Property(e => e.FechaAbono).HasColumnName("fecha_abono");
            entity.Property(e => e.ListaTotalAbonos)
                .HasPrecision(10, 2)
                .HasColumnName("lista_total_abonos");
            entity.Property(e => e.NumAbono).HasColumnName("num_abono");
            entity.Property(e => e.PrecioPagar)
                .HasPrecision(10, 2)
                .HasColumnName("precio_pagar");
            entity.Property(e => e.ReparacionId).HasColumnName("reparacion_id");

            entity.HasOne(d => d.Reparacion).WithMany(p => p.Abonos)
                .HasForeignKey(d => d.ReparacionId)
                .HasConstraintName("abonos_reparacion_id_fkey");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categorias_pkey");

            entity.ToTable("categorias");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(50)
                .HasColumnName("nombre_categoria");
            entity.Property(e => e.TipoCategoria)
                .HasMaxLength(50)
                .HasColumnName("tipo_categoria");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clientes_pkey");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.Documento, "clientes_documento_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .HasColumnName("apellido");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .HasColumnName("correo");
            entity.Property(e => e.Documento).HasColumnName("documento");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("nombre");
            entity.Property(e => e.TipoDoc)
                .HasMaxLength(10)
                .HasColumnName("tipo_doc");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("compras_pkey");

            entity.ToTable("compras");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasColumnName("estado");
            entity.Property(e => e.FechaCompra).HasColumnName("fecha_compra");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.Iva)
                .HasPrecision(10, 2)
                .HasColumnName("iva");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(20)
                .HasColumnName("metodo_pago");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Compras)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("compras_proveedor_id_fkey");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("detalle_compra_pkey");

            entity.ToTable("detalle_compra");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CompraId).HasColumnName("compra_id");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.SubtotalItems)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal_items");

            entity.HasOne(d => d.Compra).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.CompraId)
                .HasConstraintName("detalle_compra_compra_id_fkey");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("detalle_compra_producto_id_fkey");
        });

        modelBuilder.Entity<Fidelizacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fidelizacion_pkey");

            entity.ToTable("fidelizacion");

            entity.HasIndex(e => e.ClienteId, "fidelizacion_cliente_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fichos).HasColumnName("fichos");
            entity.Property(e => e.FichosNa).HasColumnName("fichos_na");
            entity.Property(e => e.Horas).HasColumnName("horas");
            entity.Property(e => e.TipoFicho).HasColumnName("tipo_ficho");

            entity.HasOne(d => d.Cliente).WithOne(p => p.Fidelizacion)
                .HasForeignKey<Fidelizacion>(d => d.ClienteId)
                .HasConstraintName("fidelizacion_cliente_id_fkey");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("permiso_pkey");

            entity.ToTable("permiso");

            entity.Property(e => e.IdPermiso).HasColumnName("id_permiso");
            entity.Property(e => e.NombrePermiso)
                .HasMaxLength(50)
                .HasColumnName("nombre_permiso");
        });

        modelBuilder.Entity<Permisoxrol>(entity =>
        {
            entity.HasKey(e => e.IdPermisoRol).HasName("permisoxrol_pkey");

            entity.ToTable("permisoxrol");

            entity.Property(e => e.IdPermisoRol).HasColumnName("id_permiso_rol");
            entity.Property(e => e.FkPermiso).HasColumnName("fk_permiso");
            entity.Property(e => e.FkRol).HasColumnName("fk_rol");

            entity.HasOne(d => d.FkPermisoNavigation).WithMany(p => p.Permisoxrols)
                .HasForeignKey(d => d.FkPermiso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("permisoxrol_fk_permiso_fkey");

            entity.HasOne(d => d.FkRolNavigation).WithMany(p => p.Permisoxrols)
                .HasForeignKey(d => d.FkRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("permisoxrol_fk_rol_fkey");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productos_pkey");

            entity.ToTable("productos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("productos_categoria_id_fkey");
        });

        modelBuilder.Entity<Productoxventum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productoxventa_pkey");

            entity.ToTable("productoxventa");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.ValorTotal)
                .HasPrecision(10, 2)
                .HasColumnName("valor_total");
            entity.Property(e => e.ValorUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("valor_unitario");
            entity.Property(e => e.VentaId).HasColumnName("venta_id");

            entity.HasOne(d => d.Producto).WithMany(p => p.Productoxventa)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("productoxventa_producto_id_fkey");

            entity.HasOne(d => d.Venta).WithMany(p => p.Productoxventa)
                .HasForeignKey(d => d.VentaId)
                .HasConstraintName("productoxventa_venta_id_fkey");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("proveedores_pkey");

            entity.ToTable("proveedores");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .HasColumnName("apellidos");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .HasColumnName("correo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .HasColumnName("direccion");
            entity.Property(e => e.Nombres)
                .HasMaxLength(50)
                .HasColumnName("nombres");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(20)
                .HasColumnName("numero_documento");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(100)
                .HasColumnName("razon_social");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(10)
                .HasColumnName("tipo_documento");
            entity.Property(e => e.TipoPersona)
                .HasMaxLength(20)
                .HasColumnName("tipo_persona");
        });

        modelBuilder.Entity<Reparacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reparacion_pkey");

            entity.ToTable("reparacion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.DetallesDano)
                .HasMaxLength(500)
                .HasColumnName("detalles_dano");
            entity.Property(e => e.DetallesSolucion)
                .HasMaxLength(500)
                .HasColumnName("detalles_solucion");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .HasColumnName("direccion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.FechaReparacion).HasColumnName("fecha_reparacion");
            entity.Property(e => e.Prioridad).HasColumnName("prioridad");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.TipoReparacion).HasColumnName("tipo_reparacion");
            entity.Property(e => e.Valor)
                .HasPrecision(10, 2)
                .HasColumnName("valor");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Reparacions)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("reparacion_cliente_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("servicios_pkey");

            entity.ToTable("servicios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Detalles)
                .HasMaxLength(300)
                .HasColumnName("detalles");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("servicios_categoria_id_fkey");
        });

        modelBuilder.Entity<Servicioxventum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("servicioxventa_pkey");

            entity.ToTable("servicioxventa");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Detalles)
                .HasMaxLength(300)
                .HasColumnName("detalles");
            entity.Property(e => e.ServicioId).HasColumnName("servicio_id");
            entity.Property(e => e.ValorTotal)
                .HasPrecision(10, 2)
                .HasColumnName("valor_total");
            entity.Property(e => e.VentaId).HasColumnName("venta_id");

            entity.HasOne(d => d.Servicio).WithMany(p => p.Servicioxventa)
                .HasForeignKey(d => d.ServicioId)
                .HasConstraintName("servicioxventa_servicio_id_fkey");

            entity.HasOne(d => d.Venta).WithMany(p => p.Servicioxventa)
                .HasForeignKey(d => d.VentaId)
                .HasConstraintName("servicioxventa_venta_id_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Celular)
                .HasMaxLength(15)
                .HasColumnName("celular");
            entity.Property(e => e.CodigoExpira).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CodigoRecuperacion).HasMaxLength(10);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(60)
                .HasColumnName("contrasena");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .HasColumnName("direccion");
            entity.Property(e => e.Documento)
                .HasMaxLength(20)
                .HasColumnName("documento");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.FkRol).HasColumnName("fk_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.TipoDoc)
                .HasMaxLength(20)
                .HasColumnName("tipo_doc");

            entity.HasOne(d => d.FkRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.FkRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuarios_fk_rol_fkey");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ventas_pkey");

            entity.ToTable("ventas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Venta)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("ventas_cliente_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
