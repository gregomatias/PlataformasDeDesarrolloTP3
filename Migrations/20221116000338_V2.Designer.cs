﻿// <auto-generated />
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
    [Migration("20221116000338_V2")]
    partial class V2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TP1.Usuario", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("_id"));

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

                    b.HasKey("_id");

                    b.ToTable("Usuarios", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
