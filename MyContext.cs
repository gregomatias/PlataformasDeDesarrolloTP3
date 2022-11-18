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

        public DbSet<TarjetaDeCredito> tarjetas { get; set; }
        public DbSet<Pago> pagos { get; set; }
        public DbSet<Movimiento> movimientos { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Properties.Resources.stringDeConexion);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //nombre de la tabla
            modelBuilder.Entity<Usuario>()
                .ToTable("Usuario")
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
            modelBuilder.Ignore<Banco>();

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



            //DEFINICIÓN DE LA RELACIÓN MANY TO MANY USUARIO <-> PAIS
            modelBuilder.Entity<Usuario>()
                .HasMany(U => U.cajas)
                .WithMany(P => P.titulares)
                .UsingEntity<UsuarioCajaDeAhorro>(
                    eup => eup.HasOne(up => up.caja).WithMany(p => p.usuarioCajas).HasForeignKey(u => u.id_caja),
                    eup => eup.HasOne(up => up.user).WithMany(u => u.usuarioCajas).HasForeignKey(u => u.id_usuario),
                    eup => eup.HasKey(k => new { k.id_caja , k.id_usuario})
                );

                //nombre de la tabla
            modelBuilder.Entity<PlazoFijo>()
                .ToTable("Plazo_fijo")
                .HasKey(c => c._id_plazoFijo);
            //propiedades de los datos
            modelBuilder.Entity<PlazoFijo>(
                plazo =>
                {
                    plazo.Property(c => c._id_usuario).HasColumnType("int");
                    plazo.Property(c => c._monto).HasColumnType("float");
                    plazo.Property(c => c._fechaIni).HasColumnType("datetime");
                    plazo.Property(c => c._fechaFin).HasColumnType("datetime");
                    plazo.Property(c => c._tasa).HasColumnType("float");
                    plazo.Property(c => c._pagado).HasColumnType("bit");
                });

            //DEFINICIÓN DE LA RELACIÓN ONE TO MANY USUARIO -> DOMICILIO
            modelBuilder.Entity<PlazoFijo>()
            .HasOne(D => D._titular)
            .WithMany(U => U._plazosFijos)
            .HasForeignKey(D => D._id_usuario)
            .OnDelete(DeleteBehavior.Cascade);


            //nombre de la tabla
            modelBuilder.Entity<TarjetaDeCredito>()
                .ToTable("Tarjeta_credito")
                .HasKey(c => c._id_tarjeta);
            //propiedades de los datos
            modelBuilder.Entity<TarjetaDeCredito>(
                plazo =>
                {
                    plazo.Property(c => c._id_usuario).HasColumnType("int");
                    plazo.Property(c => c._numero).HasColumnType("nvarchar(200)");
                    plazo.Property(c => c._codigoV).HasColumnType("int");
                    plazo.Property(c => c._limite).HasColumnType("float");
                    plazo.Property(c => c._consumos).HasColumnType("float");
                });

            //DEFINICIÓN DE LA RELACIÓN ONE TO MANY USUARIO -> DOMICILIO
            modelBuilder.Entity<TarjetaDeCredito>()
            .HasOne(D => D._titular)
            .WithMany(U => U._tarjetas)
            .HasForeignKey(D => D._id_usuario)
            .OnDelete(DeleteBehavior.Cascade);

            //nombre de la tabla
            modelBuilder.Entity<Pago>()
                .ToTable("Pago")
                .HasKey(c => c._id_pago);
            //propiedades de los datos
            modelBuilder.Entity<Pago>(
                plazo =>
                {
                    plazo.Property(c => c._id_usuario).HasColumnType("int");
                    plazo.Property(c => c._monto).HasColumnType("float");
                    plazo.Property(c => c._pagado).HasColumnType("bit");
                    plazo.Property(c => c._metodo).HasColumnType("nvarchar(200)");
                    plazo.Property(c => c._detalle).HasColumnType("nvarchar(200)");
                    plazo.Property(c => c._id_metodo).HasColumnType("bigint");
                });

            //DEFINICIÓN DE LA RELACIÓN ONE TO MANY USUARIO -> DOMICILIO
            modelBuilder.Entity<Pago>()
            .HasOne(D => D._usuario)
            .WithMany(U => U._pagos)
            .HasForeignKey(D => D._id_usuario)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Movimiento>()
                       .ToTable("Movimiento")
                .HasKey(M => M._id_Movimiento);

            //DEFINICIÓN DE LA RELACIÓN ONE TO MANY Movimiento>>CajaDeAhorro
            modelBuilder.Entity<Movimiento>()
            .HasOne(M => M._cajaDeAhorro)
            .WithMany(C => C._movimientos)
            .HasForeignKey(M => M._id_CajaDeAhorro)
            .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
