using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP1.Migrations
{
    /// <inheritdoc />
    public partial class AgregaMovimiento4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movimiento",
                columns: table => new
                {
                    idMovimiento = table.Column<int>(name: "_id_Movimiento", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idCajaDeAhorro = table.Column<int>(name: "_id_CajaDeAhorro", type: "int", nullable: false),
                    detalle = table.Column<string>(name: "_detalle", type: "nvarchar(max)", nullable: false),
                    monto = table.Column<double>(name: "_monto", type: "float", nullable: false),
                    fecha = table.Column<DateTime>(name: "_fecha", type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimiento", x => x.idMovimiento);
                    table.ForeignKey(
                        name: "FK_Movimiento_Caja_ahorro__id_CajaDeAhorro",
                        column: x => x.idCajaDeAhorro,
                        principalTable: "Caja_ahorro",
                        principalColumn: "_id_caja",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento__id_CajaDeAhorro",
                table: "Movimiento",
                column: "_id_CajaDeAhorro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimiento");
        }
    }
}
