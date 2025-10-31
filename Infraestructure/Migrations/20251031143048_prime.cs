using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class prime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Objetivos_Año_Mes",
                table: "Objetivos");

            migrationBuilder.DropIndex(
                name: "IX_Objetivos_Año_Mes_IsActive",
                table: "Objetivos");

            migrationBuilder.DropIndex(
                name: "IX_Objetivos_IdCabaña",
                table: "Objetivos");

            migrationBuilder.CreateIndex(
                name: "UX_Objetivos_Anual_Cabaña_Año",
                table: "Objetivos",
                columns: new[] { "IdCabaña", "Año" },
                unique: true,
                filter: "[Tipo] = 1 AND [Mes] IS NULL");

            migrationBuilder.CreateIndex(
                name: "UX_Objetivos_General_Año",
                table: "Objetivos",
                column: "Año",
                unique: true,
                filter: "[Tipo] = 0 AND [Mes] IS NULL AND [IdCabaña] IS NULL");

            migrationBuilder.CreateIndex(
                name: "UX_Objetivos_Mensual_Cabaña_AñoMes",
                table: "Objetivos",
                columns: new[] { "IdCabaña", "Año", "Mes" },
                unique: true,
                filter: "[Tipo] = 2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Objetivos_Anual_Cabaña_Año",
                table: "Objetivos");

            migrationBuilder.DropIndex(
                name: "UX_Objetivos_General_Año",
                table: "Objetivos");

            migrationBuilder.DropIndex(
                name: "UX_Objetivos_Mensual_Cabaña_AñoMes",
                table: "Objetivos");

            migrationBuilder.CreateIndex(
                name: "IX_Objetivos_Año_Mes",
                table: "Objetivos",
                columns: new[] { "Año", "Mes" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Objetivos_Año_Mes_IsActive",
                table: "Objetivos",
                columns: new[] { "Año", "Mes", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Objetivos_IdCabaña",
                table: "Objetivos",
                column: "IdCabaña");
        }
    }
}
