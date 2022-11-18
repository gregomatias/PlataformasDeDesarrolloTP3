﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TP1;

#nullable disable

namespace TP1.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20221118020454_AgregaMovimiento4")]
    partial class AgregaMovimiento4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TP1.CajaDeAhorro", b =>
                {
                    b.Property<int>("_id_caja")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("_id_caja"));

                    b.Property<string>("_cbu")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<double>("_saldo")
                        .HasColumnType("float");

                    b.HasKey("_id_caja");

                    b.ToTable("Caja_ahorro", (string)null);
                });

            modelBuilder.Entity("TP1.Movimiento", b =>
                {
                    b.Property<int>("_id_Movimiento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("_id_Movimiento"));

                    b.Property<string>("_detalle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("_fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("_id_CajaDeAhorro")
                        .HasColumnType("int");

                    b.Property<double>("_monto")
                        .HasColumnType("float");

                    b.HasKey("_id_Movimiento");

                    b.HasIndex("_id_CajaDeAhorro");

                    b.ToTable("Movimiento", (string)null);
                });

            modelBuilder.Entity("TP1.Pago", b =>
                {
                    b.Property<int>("_id_pago")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("_id_pago"));

                    b.Property<string>("_detalle")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("_id_metodo")
                        .HasColumnType("bigint");

                    b.Property<int>("_id_usuario")
                        .HasColumnType("int");

                    b.Property<string>("_metodo")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<double>("_monto")
                        .HasColumnType("float");

                    b.Property<bool>("_pagado")
                        .HasColumnType("bit");

                    b.HasKey("_id_pago");

                    b.HasIndex("_id_usuario");

                    b.ToTable("Pago", (string)null);
                });

            modelBuilder.Entity("TP1.PlazoFijo", b =>
                {
                    b.Property<int>("_id_plazoFijo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("_id_plazoFijo"));

                    b.Property<DateTime>("_fechaFin")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("_fechaIni")
                        .HasColumnType("datetime");

                    b.Property<int>("_id_usuario")
                        .HasColumnType("int");

                    b.Property<double>("_monto")
                        .HasColumnType("float");

                    b.Property<bool>("_pagado")
                        .HasColumnType("bit");

                    b.Property<double>("_tasa")
                        .HasColumnType("float");

                    b.HasKey("_id_plazoFijo");

                    b.HasIndex("_id_usuario");

                    b.ToTable("Plazo_fijo", (string)null);
                });

            modelBuilder.Entity("TP1.TarjetaDeCredito", b =>
                {
                    b.Property<int>("_id_tarjeta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("_id_tarjeta"));

                    b.Property<int>("_codigoV")
                        .HasColumnType("int");

                    b.Property<double>("_consumos")
                        .HasColumnType("float");

                    b.Property<int>("_id_usuario")
                        .HasColumnType("int");

                    b.Property<double>("_limite")
                        .HasColumnType("float");

                    b.Property<string>("_numero")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("_id_tarjeta");

                    b.HasIndex("_id_usuario");

                    b.ToTable("Tarjeta_credito", (string)null);
                });

            modelBuilder.Entity("TP1.Usuario", b =>
                {
                    b.Property<int>("_id_usuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("_id_usuario"));

                    b.Property<string>("_apellido")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("_bloqueado")
                        .HasColumnType("bit");

                    b.Property<int>("_dni")
                        .HasColumnType("int");

                    b.Property<bool>("_esUsuarioAdmin")
                        .HasColumnType("bit");

                    b.Property<int>("_intentosFallidos")
                        .HasColumnType("int");

                    b.Property<string>("_mail")
                        .IsRequired()
                        .HasColumnType("varchar(512)");

                    b.Property<string>("_nombre")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("_password")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("_id_usuario");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("TP1.UsuarioCajaDeAhorro", b =>
                {
                    b.Property<int>("id_caja")
                        .HasColumnType("int");

                    b.Property<int>("id_usuario")
                        .HasColumnType("int");

                    b.HasKey("id_caja", "id_usuario");

                    b.HasIndex("id_usuario");

                    b.ToTable("UsuarioCajaDeAhorro");
                });

            modelBuilder.Entity("TP1.Movimiento", b =>
                {
                    b.HasOne("TP1.CajaDeAhorro", "_cajaDeAhorro")
                        .WithMany("_movimientos")
                        .HasForeignKey("_id_CajaDeAhorro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("_cajaDeAhorro");
                });

            modelBuilder.Entity("TP1.Pago", b =>
                {
                    b.HasOne("TP1.Usuario", "_usuario")
                        .WithMany("_pagos")
                        .HasForeignKey("_id_usuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("_usuario");
                });

            modelBuilder.Entity("TP1.PlazoFijo", b =>
                {
                    b.HasOne("TP1.Usuario", "_titular")
                        .WithMany("_plazosFijos")
                        .HasForeignKey("_id_usuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("_titular");
                });

            modelBuilder.Entity("TP1.TarjetaDeCredito", b =>
                {
                    b.HasOne("TP1.Usuario", "_titular")
                        .WithMany("_tarjetas")
                        .HasForeignKey("_id_usuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("_titular");
                });

            modelBuilder.Entity("TP1.UsuarioCajaDeAhorro", b =>
                {
                    b.HasOne("TP1.CajaDeAhorro", "caja")
                        .WithMany("usuarioCajas")
                        .HasForeignKey("id_caja")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TP1.Usuario", "user")
                        .WithMany("usuarioCajas")
                        .HasForeignKey("id_usuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("caja");

                    b.Navigation("user");
                });

            modelBuilder.Entity("TP1.CajaDeAhorro", b =>
                {
                    b.Navigation("_movimientos");

                    b.Navigation("usuarioCajas");
                });

            modelBuilder.Entity("TP1.Usuario", b =>
                {
                    b.Navigation("_pagos");

                    b.Navigation("_plazosFijos");

                    b.Navigation("_tarjetas");

                    b.Navigation("usuarioCajas");
                });
#pragma warning restore 612, 618
        }
    }
}
