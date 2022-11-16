using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP1.Migrations
{
    /// <inheritdoc />
    public partial class CajaDeAhorrousuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_id",
                table: "Usuarios",
                newName: "_id_usuario");

            migrationBuilder.RenameColumn(
                name: "_id",
                table: "Caja_ahorro",
                newName: "_id_caja");

            migrationBuilder.CreateTable(
                name: "UsuarioCajaDeAhorro",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioCajaDeAhorro", x => x.id);
                    table.ForeignKey(
                        name: "FK_UsuarioCajaDeAhorro_Caja_ahorro_id",
                        column: x => x.id,
                        principalTable: "Caja_ahorro",
                        principalColumn: "_id_caja",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioCajaDeAhorro_Usuarios_id",
                        column: x => x.id,
                        principalTable: "Usuarios",
                        principalColumn: "_id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioCajaDeAhorro");

            migrationBuilder.RenameColumn(
                name: "_id_usuario",
                table: "Usuarios",
                newName: "_id");

            migrationBuilder.RenameColumn(
                name: "_id_caja",
                table: "Caja_ahorro",
                newName: "_id");
        }
    }
}
