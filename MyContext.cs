using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TP1
{
    internal class MyContext : DbContext
    {
        public MyContext() { }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<CajaDeAhorro> cajas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Properties.Resources.stringDeConexion);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //nombre de la tabla
            modelBuilder.Entity<Usuario>()
                .ToTable("Usuarios")
                .HasKey(u => u._id_usuario);
            //propiedades de los datos
            modelBuilder.Entity<Usuario>(
            usr =>
                {
                    usr.Property(u => u._dni).HasColumnType("int");
                    usr.Property(u => u._dni).IsRequired(true);
                    usr.Property(u => u._nombre).HasColumnType("varchar(50)");
                    usr.Property(u => u._apellido).HasColumnType("varchar(50)");
                    usr.Property(u => u._mail).HasColumnType("varchar(512)");
                    usr.Property(u => u._password).HasColumnType("varchar(50)");
                    usr.Property(u => u._esUsuarioAdmin).HasColumnType("bit");
                    usr.Property(u => u._bloqueado).HasColumnType("bit");
                    usr.Property(u => u._intentosFallidos).HasColumnType("int");
                })
            ;
            //Ignoro, no agrego UsuarioManager a la base de datos
            modelBuilder.Ignore<UsuarioManager>();

            //nombre de la tabla
            modelBuilder.Entity<CajaDeAhorro>()
                .ToTable("Caja_ahorro")
                .HasKey(c => c._id_caja);
            //propiedades de los datos
            modelBuilder.Entity<CajaDeAhorro>(
                caja =>
                {
                    caja.Property(c => c._cbu).HasColumnType("nvarchar(200)");
                    caja.Property(c => c._cbu).IsRequired(true);
                    caja.Property(c => c._saldo).HasColumnType("float");
                });
            //Ignoro, no agrego UsuarioManager a la base de datos
            modelBuilder.Ignore<CajaDeAhorroManager>();


            //DEFINICIÓN DE LA RELACIÓN MANY TO MANY USUARIO <-> PAIS
            modelBuilder.Entity<Usuario>()
                .HasMany(U => U.cajas)
                .WithMany(P => P.titulares)
                .UsingEntity<UsuarioCajaDeAhorro>(
                    eup => eup.HasOne(up => up.caja).WithMany(p => p.usuarioCajas).HasForeignKey(u => u.id_caja),
                    eup => eup.HasOne(up => up.user).WithMany(u => u.usuarioCajas).HasForeignKey(u => u.id_usuario),
                    eup => eup.HasKey(k => new { k.id_caja , k.id_usuario})
                );
        }

    }
}
