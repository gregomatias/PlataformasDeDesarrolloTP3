using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP1.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Caja_ahorro",
                columns: table => new
                {
                    idcaja = table.Column<int>(name: "_id_caja", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cbu = table.Column<string>(name: "_cbu", type: "nvarchar(200)", nullable: false),
                    saldo = table.Column<double>(name: "_saldo", type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caja_ahorro", x => x.idcaja);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    idusuario = table.Column<int>(name: "_id_usuario", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dni = table.Column<int>(name: "_dni", type: "int", nullable: false),
                    nombre = table.Column<string>(name: "_nombre", type: "varchar(50)", nullable: false),
                    apellido = table.Column<string>(name: "_apellido", type: "varchar(50)", nullable: false),
                    mail = table.Column<string>(name: "_mail", type: "varchar(512)", nullable: false),
                    password = table.Column<string>(name: "_password", type: "varchar(50)", nullable: false),
                    intentosFallidos = table.Column<int>(name: "_intentosFallidos", type: "int", nullable: false),
                    esUsuarioAdmin = table.Column<bool>(name: "_esUsuarioAdmin", type: "bit", nullable: false),
                    bloqueado = table.Column<bool>(name: "_bloqueado", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.idusuario);
                });

            migrationBuilder.CreateTable(
                name: "Plazo_fijo",
                columns: table => new
                {
                    idplazoFijo = table.Column<int>(name: "_id_plazoFijo", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idusuario = table.Column<int>(name: "_id_usuario", type: "int", nullable: false),
                    monto = table.Column<double>(name: "_monto", type: "float", nullable: false),
                    fechaIni = table.Column<DateTime>(name: "_fechaIni", type: "datetime", nullable: false),
                    fechaFin = table.Column<DateTime>(name: "_fechaFin", type: "datetime", nullable: false),
                    tasa = table.Column<double>(name: "_tasa", type: "float", nullable: false),
                    pagado = table.Column<bool>(name: "_pagado", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plazo_fijo", x => x.idplazoFijo);
                    table.ForeignKey(
                        name: "FK_Plazo_fijo_Usuarios__id_usuario",
                        column: x => x.idusuario,
                        principalTable: "Usuarios",
                        principalColumn: "_id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioCajaDeAhorro",
                columns: table => new
                {
                    idcaja = table.Column<int>(name: "id_caja", type: "int", nullable: false),
                    idusuario = table.Column<int>(name: "id_usuario", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioCajaDeAhorro", x => new { x.idcaja, x.idusuario });
                    table.ForeignKey(
                        name: "FK_UsuarioCajaDeAhorro_Caja_ahorro_id_caja",
                        column: x => x.idcaja,
                        principalTable: "Caja_ahorro",
                        principalColumn: "_id_caja",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioCajaDeAhorro_Usuarios_id_usuario",
                        column: x => x.idusuario,
                        principalTable: "Usuarios",
                        principalColumn: "_id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plazo_fijo__id_usuario",
                table: "Plazo_fijo",
                column: "_id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCajaDeAhorro_id_usuario",
                table: "UsuarioCajaDeAhorro",
                column: "id_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plazo_fijo");

            migrationBuilder.DropTable(
                name: "UsuarioCajaDeAhorro");

            migrationBuilder.DropTable(
                name: "Caja_ahorro");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
