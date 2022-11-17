using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP1.Migrations
{
    /// <inheritdoc />
    public partial class pagos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plazo_fijo_Usuarios__id_usuario",
                table: "Plazo_fijo");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarjeta_credito_Usuarios__id_usuario",
                table: "Tarjeta_credito");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Usuarios_id_usuario",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                newName: "Usuario");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "_id_usuario");

            migrationBuilder.CreateTable(
                name: "Pago",
                columns: table => new
                {
                    idpago = table.Column<int>(name: "_id_pago", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idusuario = table.Column<int>(name: "_id_usuario", type: "int", nullable: false),
                    monto = table.Column<double>(name: "_monto", type: "float", nullable: false),
                    pagado = table.Column<bool>(name: "_pagado", type: "bit", nullable: false),
                    metodo = table.Column<string>(name: "_metodo", type: "nvarchar(200)", nullable: false),
                    detalle = table.Column<string>(name: "_detalle", type: "nvarchar(200)", nullable: false),
                    idmetodo = table.Column<int>(name: "_id_metodo", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pago", x => x.idpago);
                    table.ForeignKey(
                        name: "FK_Pago_Usuario__id_usuario",
                        column: x => x.idusuario,
                        principalTable: "Usuario",
                        principalColumn: "_id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pago__id_usuario",
                table: "Pago",
                column: "_id_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Plazo_fijo_Usuario__id_usuario",
                table: "Plazo_fijo",
                column: "_id_usuario",
                principalTable: "Usuario",
                principalColumn: "_id_usuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tarjeta_credito_Usuario__id_usuario",
                table: "Tarjeta_credito",
                column: "_id_usuario",
                principalTable: "Usuario",
                principalColumn: "_id_usuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Usuario_id_usuario",
                table: "UsuarioCajaDeAhorro",
                column: "id_usuario",
                principalTable: "Usuario",
                principalColumn: "_id_usuario",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plazo_fijo_Usuario__id_usuario",
                table: "Plazo_fijo");

            migrationBuilder.DropForeignKey(
                name: "FK_Tarjeta_credito_Usuario__id_usuario",
                table: "Tarjeta_credito");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Usuario_id_usuario",
                table: "UsuarioCajaDeAhorro");

            migrationBuilder.DropTable(
                name: "Pago");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.RenameTable(
                name: "Usuario",
                newName: "Usuarios");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuarios",
                table: "Usuarios",
                column: "_id_usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Plazo_fijo_Usuarios__id_usuario",
                table: "Plazo_fijo",
                column: "_id_usuario",
                principalTable: "Usuarios",
                principalColumn: "_id_usuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tarjeta_credito_Usuarios__id_usuario",
                table: "Tarjeta_credito",
                column: "_id_usuario",
                principalTable: "Usuarios",
                principalColumn: "_id_usuario",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioCajaDeAhorro_Usuarios_id_usuario",
                table: "UsuarioCajaDeAhorro",
                column: "id_usuario",
                principalTable: "Usuarios",
                principalColumn: "_id_usuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
