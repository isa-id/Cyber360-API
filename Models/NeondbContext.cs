using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend.Models;

public partial class NeondbContext : DbContext
{
    public NeondbContext()
    {
    }

    public NeondbContext(DbContextOptions<NeondbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriaProducto> CategoriaProductos { get; set; }

    public virtual DbSet<CategoriaServicio> CategoriaServicios { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<ComprasXProducto> ComprasXProductos { get; set; }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<ImagenesProducto> ImagenesProductos { get; set; }

    public virtual DbSet<ImagenesServicio> ImagenesServicios { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Permisoxrol> Permisoxrols { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Reparacione> Reparaciones { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<Servicioxinsumo> Servicioxinsumos { get; set; }

    public virtual DbSet<Servicioxventum> Servicioxventa { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<Ventaxproducto> Ventaxproductos { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseNpgsql("Host=ep-twilight-leaf-a81vdkl5-pooler.eastus2.azure.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_DId51CeRfMQg;SSL Mode=Require");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaProducto).HasName("categoria_producto_pkey");

            entity.ToTable("categoria_producto");

            entity.Property(e => e.IdCategoriaProducto).HasColumnName("id_categoria_producto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(25)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<CategoriaServicio>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaServicio).HasName("categoria_servicio_pkey");

            entity.ToTable("categoria_servicio");

            entity.Property(e => e.IdCategoriaServicio).HasColumnName("id_categoria_servicio");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(25)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("clientes_pkey");

            entity.ToTable("clientes");

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Celular)
                .HasMaxLength(15)
                .HasColumnName("celular");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .HasColumnName("direccion");
            entity.Property(e => e.Documento)
                .HasMaxLength(20)
                .HasColumnName("documento");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Fichos).HasColumnName("fichos");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(50)
                .HasColumnName("nombre_completo");
            entity.Property(e => e.Tiempo)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("tiempo");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(20)
                .HasColumnName("tipo_documento");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("compras_pkey");

            entity.ToTable("compras");

            entity.Property(e => e.IdCompra).HasColumnName("id_compra");
            entity.Property(e => e.FechaCompra).HasColumnName("fecha_compra");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.FkProveedor).HasColumnName("fk_proveedor");
            entity.Property(e => e.NumeroCompra).HasColumnName("numero_compra");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.FkProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.FkProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_compras_proveedor");
        });

        modelBuilder.Entity<ComprasXProducto>(entity =>
        {
            entity.HasKey(e => e.IdComprasXProductos).HasName("compras_x_productos_pkey");

            entity.ToTable("compras_x_productos");

            entity.Property(e => e.IdComprasXProductos).HasColumnName("id_compras_x_productos");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FkCompra).HasColumnName("fk_compra");
            entity.Property(e => e.FkProducto).HasColumnName("fk_producto");
            entity.Property(e => e.PrecioUnit)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unit");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");

            entity.HasOne(d => d.FkCompraNavigation).WithMany(p => p.ComprasXProductos)
                .HasForeignKey(d => d.FkCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("compras_x_productos_fk_compra_fkey");

            entity.HasOne(d => d.FkProductoNavigation).WithMany(p => p.ComprasXProductos)
                .HasForeignKey(d => d.FkProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("compras_x_productos_fk_producto_fkey");
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.IdEquipo).HasName("equipos_pkey");

            entity.ToTable("equipos");

            entity.Property(e => e.IdEquipo).HasColumnName("id_equipo");
            entity.Property(e => e.Categoria)
                .HasMaxLength(50)
                .HasColumnName("categoria");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Tiempo).HasColumnName("tiempo");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("estados_pkey");

            entity.ToTable("estados");

            entity.Property(e => e.IdEstado).HasColumnName("id_estado");
            entity.Property(e => e.NombreEstado)
                .HasMaxLength(50)
                .HasColumnName("nombre_estado");
        });

        modelBuilder.Entity<ImagenesProducto>(entity =>
        {
            entity.HasKey(e => e.IdImagen).HasName("imagenes_producto_pkey");

            entity.ToTable("imagenes_producto");

            entity.Property(e => e.IdImagen).HasColumnName("id_imagen");
            entity.Property(e => e.Url)
                .HasMaxLength(150)
                .HasColumnName("url");
        });

        modelBuilder.Entity<ImagenesServicio>(entity =>
        {
            entity.HasKey(e => e.IdImagen).HasName("imagenes_servicio_pkey");

            entity.ToTable("imagenes_servicio");

            entity.Property(e => e.IdImagen).HasColumnName("id_imagen");
            entity.Property(e => e.Url)
                .HasMaxLength(150)
                .HasColumnName("url");
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
            entity.HasKey(e => e.IdProducto).HasName("productos_pkey");

            entity.ToTable("productos");

            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.FkCategoria).HasColumnName("fk_categoria");
            entity.Property(e => e.FkImagen).HasColumnName("fk_imagen");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.FkCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.FkCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productos_categoria");

            entity.HasOne(d => d.FkImagenNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.FkImagen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_productos_imagen");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("proveedores_pkey");

            entity.ToTable("proveedores");

            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.Contacto)
                .HasMaxLength(50)
                .HasColumnName("contacto");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .HasColumnName("direccion");
            entity.Property(e => e.Documento)
                .HasMaxLength(20)
                .HasColumnName("documento");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.NombreRa)
                .HasMaxLength(50)
                .HasColumnName("nombre_ra");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(20)
                .HasColumnName("tipo_documento");
        });

        modelBuilder.Entity<Reparacione>(entity =>
        {
            entity.HasKey(e => e.IdReparacion).HasName("reparaciones_pkey");

            entity.ToTable("reparaciones");

            entity.HasIndex(e => e.Fecha, "reparaciones_fecha_key").IsUnique();

            entity.Property(e => e.IdReparacion).HasColumnName("id_reparacion");
            entity.Property(e => e.Adelanto)
                .HasPrecision(10, 2)
                .HasColumnName("adelanto");
            entity.Property(e => e.DetallesDano)
                .HasMaxLength(200)
                .HasColumnName("detalles_dano");
            entity.Property(e => e.DetallesSolucion)
                .HasMaxLength(200)
                .HasColumnName("detalles_solucion");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.FechaEstimada).HasColumnName("fecha_estimada");
            entity.Property(e => e.FechaReparacion).HasColumnName("fecha_reparacion");
            entity.Property(e => e.FkCliente).HasColumnName("fk_cliente");
            entity.Property(e => e.FkEstado).HasColumnName("fk_estado");
            entity.Property(e => e.TipoReparacion)
                .HasMaxLength(30)
                .HasColumnName("tipo_reparacion");
            entity.Property(e => e.Valor)
                .HasPrecision(10, 2)
                .HasColumnName("valor");
            entity.Property(e => e.ValorTotal)
                .HasPrecision(10, 2)
                .HasColumnName("valor_total");

            entity.HasOne(d => d.FkClienteNavigation).WithMany(p => p.Reparaciones)
                .HasForeignKey(d => d.FkCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reparaciones_fk_cliente_fkey");

            entity.HasOne(d => d.FkEstadoNavigation).WithMany(p => p.Reparaciones)
                .HasForeignKey(d => d.FkEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reparaciones_fk_estado_fkey");
        });

            modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .HasColumnName("nombre_rol");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)        // ajusta tamaño si aplica
                .HasColumnName("descripcion");

            entity.Property(e => e.Activo)
                .HasColumnName("activo")
                .IsRequired();
        });



        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("servicios_pkey");

            entity.ToTable("servicios");

            entity.Property(e => e.IdServicio).HasColumnName("id_servicio");
            entity.Property(e => e.Detalles)
                .HasMaxLength(200)
                .HasColumnName("detalles");
            entity.Property(e => e.FkCategoriaServicio).HasColumnName("fk_categoria_servicio");
            entity.Property(e => e.FkEquipo).HasColumnName("fk_equipo");
            entity.Property(e => e.FkImagen).HasColumnName("fk_imagen");
            entity.Property(e => e.NombreServicio)
                .HasMaxLength(50)
                .HasColumnName("nombre_servicio");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");

            entity.HasOne(d => d.FkCategoriaServicioNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.FkCategoriaServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("servicios_fk_categoria_servicio_fkey");

            entity.HasOne(d => d.FkEquipoNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.FkEquipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("servicios_fk_equipo_fkey");

            entity.HasOne(d => d.FkImagenNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.FkImagen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("servicios_fk_imagen_fkey");
        });

        modelBuilder.Entity<Servicioxinsumo>(entity =>
        {
            entity.HasKey(e => e.IdServicioxinsumo).HasName("servicioxinsumo_pkey");

            entity.ToTable("servicioxinsumo");

            entity.Property(e => e.IdServicioxinsumo).HasColumnName("id_servicioxinsumo");
            entity.Property(e => e.FkProducto).HasColumnName("fk_producto");
            entity.Property(e => e.FkServicio).HasColumnName("fk_servicio");

            entity.HasOne(d => d.FkProductoNavigation).WithMany(p => p.Servicioxinsumos)
                .HasForeignKey(d => d.FkProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("servicioxinsumo_fk_producto_fkey");

            entity.HasOne(d => d.FkServicioNavigation).WithMany(p => p.Servicioxinsumos)
                .HasForeignKey(d => d.FkServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("servicioxinsumo_fk_servicio_fkey");
        });

        modelBuilder.Entity<Servicioxventum>(entity =>
        {
            entity.HasKey(e => e.IdServicioxventa).HasName("servicioxventa_pkey");

            entity.ToTable("servicioxventa");

            entity.Property(e => e.IdServicioxventa).HasColumnName("id_servicioxventa");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FkServicio).HasColumnName("fk_servicio");
            entity.Property(e => e.FkVenta).HasColumnName("fk_venta");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");

            entity.HasOne(d => d.FkServicioNavigation).WithMany(p => p.Servicioxventa)
                .HasForeignKey(d => d.FkServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("servicioxventa_fk_servicio_fkey");

            entity.HasOne(d => d.FkVentaNavigation).WithMany(p => p.Servicioxventa)
                .HasForeignKey(d => d.FkVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("servicioxventa_fk_venta_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Celular)
                .HasMaxLength(15)
                .HasColumnName("celular");
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
            entity.HasKey(e => e.IdVenta).HasName("ventas_pkey");

            entity.ToTable("ventas");

            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.FechaVenta).HasColumnName("fecha_venta");
            entity.Property(e => e.FkCliente).HasColumnName("fk_cliente");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .HasColumnName("metodo_pago");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.FkClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.FkCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ventas_fk_cliente_fkey");
        });

        modelBuilder.Entity<Ventaxproducto>(entity =>
        {
            entity.HasKey(e => e.IdVentaxproducto).HasName("ventaxproducto_pkey");

            entity.ToTable("ventaxproducto");

            entity.Property(e => e.IdVentaxproducto).HasColumnName("id_ventaxproducto");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FkProducto).HasColumnName("fk_producto");
            entity.Property(e => e.FkVenta).HasColumnName("fk_venta");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");

            entity.HasOne(d => d.FkVentaNavigation).WithMany(p => p.Ventaxproductos)
                .HasForeignKey(d => d.FkVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ventaxproducto_fk_venta_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
