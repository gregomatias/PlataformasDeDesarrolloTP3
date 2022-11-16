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
                name: "Usuarios",
                columns: table => new
                {
                    id = table.Column<int>(name: "_id", type: "int", nullable: false)
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
                    table.PrimaryKey("PK_Usuarios", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
