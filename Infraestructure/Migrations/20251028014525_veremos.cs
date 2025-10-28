using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class veremos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cancelacion_Reservas_ReservaId",
                table: "Cancelacion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Objetivo",
                table: "Objetivo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cancelacion",
                table: "Cancelacion");

            migrationBuilder.RenameTable(
                name: "Objetivo",
                newName: "Objetivos");

            migrationBuilder.RenameTable(
                name: "Cancelacion",
                newName: "Cancelaciones");

            migrationBuilder.RenameIndex(
                name: "IX_Objetivo_Año_Mes_IsActive",
                table: "Objetivos",
                newName: "IX_Objetivos_Año_Mes_IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_Objetivo_Año_Mes",
                table: "Objetivos",
                newName: "IX_Objetivos_Año_Mes");

            migrationBuilder.RenameIndex(
                name: "IX_Cancelacion_ReservaId_Fecha",
                table: "Cancelaciones",
                newName: "IX_Cancelaciones_ReservaId_Fecha");

            migrationBuilder.RenameIndex(
                name: "IX_Cancelacion_ReservaId",
                table: "Cancelaciones",
                newName: "IX_Cancelaciones_ReservaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Objetivos",
                table: "Objetivos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cancelaciones",
                table: "Cancelaciones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cancelaciones_Reservas_ReservaId",
                table: "Cancelaciones",
                column: "ReservaId",
                principalTable: "Reservas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cancelaciones_Reservas_ReservaId",
                table: "Cancelaciones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Objetivos",
                table: "Objetivos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cancelaciones",
                table: "Cancelaciones");

            migrationBuilder.RenameTable(
                name: "Objetivos",
                newName: "Objetivo");

            migrationBuilder.RenameTable(
                name: "Cancelaciones",
                newName: "Cancelacion");

            migrationBuilder.RenameIndex(
                name: "IX_Objetivos_Año_Mes_IsActive",
                table: "Objetivo",
                newName: "IX_Objetivo_Año_Mes_IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_Objetivos_Año_Mes",
                table: "Objetivo",
                newName: "IX_Objetivo_Año_Mes");

            migrationBuilder.RenameIndex(
                name: "IX_Cancelaciones_ReservaId_Fecha",
                table: "Cancelacion",
                newName: "IX_Cancelacion_ReservaId_Fecha");

            migrationBuilder.RenameIndex(
                name: "IX_Cancelaciones_ReservaId",
                table: "Cancelacion",
                newName: "IX_Cancelacion_ReservaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Objetivo",
                table: "Objetivo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cancelacion",
                table: "Cancelacion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cancelacion_Reservas_ReservaId",
                table: "Cancelacion",
                column: "ReservaId",
                principalTable: "Reservas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
