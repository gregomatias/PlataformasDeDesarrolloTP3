using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP1.Migrations
{
    /// <inheritdoc />
    public partial class tarjetas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tarjeta_credito",
                columns: table => new
                {
                    idtarjeta = table.Column<int>(name: "_id_tarjeta", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idusuario = table.Column<int>(name: "_id_usuario", type: "int", nullable: false),
                    numero = table.Column<string>(name: "_numero", type: "nvarchar(16)", nullable: false),
                    codigoV = table.Column<int>(name: "_codigoV", type: "int", nullable: false),
                    limite = table.Column<double>(name: "_limite", type: "float", nullable: false),
                    consumos = table.Column<double>(name: "_consumos", type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarjeta_credito", x => x.idtarjeta);
                    table.ForeignKey(
                        name: "FK_Tarjeta_credito_Usuarios__id_usuario",
                        column: x => x.idusuario,
                        principalTable: "Usuarios",
                        principalColumn: "_id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tarjeta_credito__id_usuario",
                table: "Tarjeta_credito",
                column: "_id_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tarjeta_credito");
        }
    }
}
