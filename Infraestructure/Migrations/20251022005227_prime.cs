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
                name: "IX_Objetivo_Año_Mes",
                table: "Objetivo");

            migrationBuilder.RenameColumn(
                name: "Activo",
                table: "Objetivo",
                newName: "IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_Objetivo_Año_Mes_Activo",
                table: "Objetivo",
                newName: "IX_Objetivo_Año_Mes_IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Objetivo_Año_Mes",
                table: "Objetivo",
                columns: new[] { "Año", "Mes" },
                unique: true,
                filter: "[IsActive] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Objetivo_Año_Mes",
                table: "Objetivo");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Objetivo",
                newName: "Activo");

            migrationBuilder.RenameIndex(
                name: "IX_Objetivo_Año_Mes_IsActive",
                table: "Objetivo",
                newName: "IX_Objetivo_Año_Mes_Activo");

            migrationBuilder.CreateIndex(
                name: "IX_Objetivo_Año_Mes",
                table: "Objetivo",
                columns: new[] { "Año", "Mes" },
                unique: true,
                filter: "[Activo] = 1");
        }
    }
}
