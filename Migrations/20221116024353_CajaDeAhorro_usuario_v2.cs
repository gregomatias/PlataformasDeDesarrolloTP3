using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP1.Migrations
{
    /// <inheritdoc />
    public partial class CajaDeAhorrousuariov2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Caja_ahorro_id",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Usuarios_id",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioCajaDeAhorro",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UsuarioCajaDeAhorro",
                newName: "id_usuario");

            migrationBuilder.AddColumn<int>(
                name: "id_caja",
                table: "UsuarioCajaDeAhorro",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioCajaDeAhorro",
                table: "UsuarioCajaDeAhorro",
                columns: new[] { "id_caja", "id_usuario" });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCajaDeAhorro_id_usuario",
                table: "UsuarioCajaDeAhorro",
                column: "id_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Caja_ahorro_id_caja",
                table: "UsuarioCajaDeAhorro",
                column: "id_caja",
                principalTable: "Caja_ahorro",
                principalColumn: "_id_caja",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Usuarios_id_usuario",
                table: "UsuarioCajaDeAhorro",
                column: "id_usuario",
                principalTable: "Usuarios",
                principalColumn: "_id_usuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Caja_ahorro_id_caja",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Usuarios_id_usuario",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioCajaDeAhorro",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.DropIndex(
                name: "IX_UsuarioCajaDeAhorro_id_usuario",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.DropColumn(
                name: "id_caja",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.RenameColumn(
                name: "id_usuario",
                table: "UsuarioCajaDeAhorro",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioCajaDeAhorro",
                table: "UsuarioCajaDeAhorro",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Caja_ahorro_id",
                table: "UsuarioCajaDeAhorro",
                column: "id",
                principalTable: "Caja_ahorro",
                principalColumn: "_id_caja",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Usuarios_id",
                table: "UsuarioCajaDeAhorro",
                column: "id",
                principalTable: "Usuarios",
                principalColumn: "_id_usuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
