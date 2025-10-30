using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class Primera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCabaña",
                table: "Objetivos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Objetivos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Objetivos_IdCabaña",
                table: "Objetivos",
                column: "IdCabaña");

            migrationBuilder.AddForeignKey(
                name: "FK_Objetivos_Cabañas_IdCabaña",
                table: "Objetivos",
                column: "IdCabaña",
                principalTable: "Cabañas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Objetivos_Cabañas_IdCabaña",
                table: "Objetivos");

            migrationBuilder.DropIndex(
                name: "IX_Objetivos_IdCabaña",
                table: "Objetivos");

            migrationBuilder.DropColumn(
                name: "IdCabaña",
                table: "Objetivos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Objetivos");
        }
    }
}
