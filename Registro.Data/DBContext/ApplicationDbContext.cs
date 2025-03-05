using Microsoft.EntityFrameworkCore;
using Registro.Model.Entities;

namespace Registro.DAL.DBContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<MenuRol> MenuRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Tipos de dato y config para la tabla Menu
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.IdMenu);
            entity.ToTable("Menu");
            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.Icono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("icono");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Url)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("url");
        });

        //config tabla Rol
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("GETDATE()")
                .HasColumnType("datetime");
        });

        //config tabla Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Clave)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.EsActivo)
                .HasDefaultValue(true)
                .HasColumnName("esActivo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombreCompleto");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol);
       
        });

        //config tabla MenuRol
        modelBuilder.Entity<MenuRol>(entity =>
        {
            entity.HasKey(e => new { e.IdMenu, e.IdRol }); 

            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.IdRol).HasColumnName("idRol");

            entity.HasOne(d => d.IdMenuNavigation)
                .WithMany(p => p.MenuRoles)
                .HasForeignKey(d => d.IdMenu);

            entity.HasOne(d => d.IdRolNavigation)
                .WithMany(p => p.MenuRoles)
                .HasForeignKey(d => d.IdRol);
        });

        base.OnModelCreating(modelBuilder);
    }
}
