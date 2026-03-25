using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVP_Turnero.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_AspNetUsers_UsuarioId1",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Profesional_AspNetUsers_UsuarioId1",
                table: "Profesional");

            migrationBuilder.DropIndex(
                name: "IX_Profesional_UsuarioId1",
                table: "Profesional");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_UsuarioId1",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Profesional");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "Clientes");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_AspNetUsers_UsuarioId",
                table: "Clientes",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profesional_AspNetUsers_UsuarioId",
                table: "Profesional",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_AspNetUsers_UsuarioId",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Profesional_AspNetUsers_UsuarioId",
                table: "Profesional");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId1",
                table: "Profesional",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId1",
                table: "Clientes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Profesional_UsuarioId1",
                table: "Profesional",
                column: "UsuarioId1");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_UsuarioId1",
                table: "Clientes",
                column: "UsuarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_AspNetUsers_UsuarioId1",
                table: "Clientes",
                column: "UsuarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profesional_AspNetUsers_UsuarioId1",
                table: "Profesional",
                column: "UsuarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
