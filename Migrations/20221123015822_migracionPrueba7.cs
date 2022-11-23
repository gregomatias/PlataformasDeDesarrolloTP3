using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TP1.Migrations
{
    /// <inheritdoc />
    public partial class migracionPrueba7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Caja_ahorro_id_caja",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Usuario_id_usuario",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.RenameColumn(
                name: "id_usuario",
                table: "UsuarioCajaDeAhorro",
                newName: "_id_usuario");

            migrationBuilder.RenameColumn(
                name: "id_caja",
                table: "UsuarioCajaDeAhorro",
                newName: "_id_caja");

            migrationBuilder.RenameIndex(
                name: "IX_UsuarioCajaDeAhorro_id_usuario",
                table: "UsuarioCajaDeAhorro",
                newName: "IX_UsuarioCajaDeAhorro__id_usuario");

            migrationBuilder.InsertData(
                table: "Caja_ahorro",
                columns: new[] { "_id_caja", "_cbu", "_saldo" },
                values: new object[,]
                {
                    { 1, "11120221121", 0.0 },
                    { 2, "22220221122", 0.0 },
                    { 3, "33320221123", 0.0 },
                    { 4, "44420221124", 0.0 },
                    { 5, "55520221125", 0.0 },
                    { 6, "66620221125", 0.0 }
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "_id_usuario", "_apellido", "_bloqueado", "_dni", "_esUsuarioAdmin", "_intentosFallidos", "_mail", "_nombre", "_password" },
                values: new object[,]
                {
                    { 1, "GREGO", false, 111, false, 0, "M@G", "MATIAS", "111" },
                    { 2, "RIVA", false, 222, false, 0, "A@R", "ALAN", "222" },
                    { 3, "VILLEGAS", false, 333, false, 0, "N@V", "NICOLAS", "333" },
                    { 4, "GOMEZ", false, 444, true, 0, "W@G", "WALTER", "444" }
                });

            migrationBuilder.InsertData(
                table: "Plazo_fijo",
                columns: new[] { "_id_plazoFijo", "_fechaFin", "_fechaIni", "_id_usuario", "_monto", "_pagado", "_tasa" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 22, 22, 58, 21, 934, DateTimeKind.Local).AddTicks(3387), new DateTime(2022, 11, 22, 22, 58, 21, 934, DateTimeKind.Local).AddTicks(3380), 1, 1000.0, false, 1.5 },
                    { 2, new DateTime(2022, 12, 22, 22, 58, 21, 934, DateTimeKind.Local).AddTicks(3394), new DateTime(2022, 11, 22, 22, 58, 21, 934, DateTimeKind.Local).AddTicks(3393), 2, 2000.0, false, 1.5 },
                    { 3, new DateTime(2022, 12, 22, 22, 58, 21, 934, DateTimeKind.Local).AddTicks(3396), new DateTime(2022, 11, 22, 22, 58, 21, 934, DateTimeKind.Local).AddTicks(3395), 3, 3000.0, false, 1.5 },
                    { 4, new DateTime(2022, 12, 22, 22, 58, 21, 934, DateTimeKind.Local).AddTicks(3397), new DateTime(2022, 11, 22, 22, 58, 21, 934, DateTimeKind.Local).AddTicks(3397), 4, 4000.0, false, 1.5 }
                });

            migrationBuilder.InsertData(
                table: "Tarjeta_credito",
                columns: new[] { "_id_tarjeta", "_codigoV", "_consumos", "_id_usuario", "_limite", "_numero" },
                values: new object[,]
                {
                    { 1, 0, 100.0, 1, 500000.0, "11120221121" },
                    { 2, 0, 900.0, 2, 400000.0, "22220221121" },
                    { 3, 0, 400.0, 3, 600000.0, "33320221121" },
                    { 4, 0, 600.0, 4, 200000.0, "44420221121" }
                });

            migrationBuilder.InsertData(
                table: "UsuarioCajaDeAhorro",
                columns: new[] { "_id_caja", "_id_usuario" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 1 },
                    { 6, 1 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Caja_ahorro__id_caja",
                table: "UsuarioCajaDeAhorro",
                column: "_id_caja",
                principalTable: "Caja_ahorro",
                principalColumn: "_id_caja",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Usuario__id_usuario",
                table: "UsuarioCajaDeAhorro",
                column: "_id_usuario",
                principalTable: "Usuario",
                principalColumn: "_id_usuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Caja_ahorro__id_caja",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Usuario__id_usuario",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.DeleteData(
                table: "Plazo_fijo",
                keyColumn: "_id_plazoFijo",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plazo_fijo",
                keyColumn: "_id_plazoFijo",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plazo_fijo",
                keyColumn: "_id_plazoFijo",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Plazo_fijo",
                keyColumn: "_id_plazoFijo",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tarjeta_credito",
                keyColumn: "_id_tarjeta",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tarjeta_credito",
                keyColumn: "_id_tarjeta",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tarjeta_credito",
                keyColumn: "_id_tarjeta",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tarjeta_credito",
                keyColumn: "_id_tarjeta",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UsuarioCajaDeAhorro",
                keyColumns: new[] { "_id_caja", "_id_usuario" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "UsuarioCajaDeAhorro",
                keyColumns: new[] { "_id_caja", "_id_usuario" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "UsuarioCajaDeAhorro",
                keyColumns: new[] { "_id_caja", "_id_usuario" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "UsuarioCajaDeAhorro",
                keyColumns: new[] { "_id_caja", "_id_usuario" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "UsuarioCajaDeAhorro",
                keyColumns: new[] { "_id_caja", "_id_usuario" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "UsuarioCajaDeAhorro",
                keyColumns: new[] { "_id_caja", "_id_usuario" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "Caja_ahorro",
                keyColumn: "_id_caja",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Caja_ahorro",
                keyColumn: "_id_caja",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Caja_ahorro",
                keyColumn: "_id_caja",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Caja_ahorro",
                keyColumn: "_id_caja",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Caja_ahorro",
                keyColumn: "_id_caja",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Caja_ahorro",
                keyColumn: "_id_caja",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "_id_usuario",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "_id_usuario",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "_id_usuario",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "_id_usuario",
                keyValue: 4);

            migrationBuilder.RenameColumn(
                name: "_id_usuario",
                table: "UsuarioCajaDeAhorro",
                newName: "id_usuario");

            migrationBuilder.RenameColumn(
                name: "_id_caja",
                table: "UsuarioCajaDeAhorro",
                newName: "id_caja");

            migrationBuilder.RenameIndex(
                name: "IX_UsuarioCajaDeAhorro__id_usuario",
                table: "UsuarioCajaDeAhorro",
                newName: "IX_UsuarioCajaDeAhorro_id_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Caja_ahorro_id_caja",
                table: "UsuarioCajaDeAhorro",
                column: "id_caja",
                principalTable: "Caja_ahorro",
                principalColumn: "_id_caja",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Usuario_id_usuario",
                table: "UsuarioCajaDeAhorro",
                column: "id_usuario",
                principalTable: "Usuario",
                principalColumn: "_id_usuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
