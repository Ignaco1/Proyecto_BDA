using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservas_IdCabaña",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "UX_Objetivos_Anual_Cabaña_Año",
                table: "Objetivos");

            migrationBuilder.DropIndex(
                name: "UX_Objetivos_General_Año",
                table: "Objetivos");

            migrationBuilder.DropIndex(
                name: "UX_Objetivos_Mensual_Cabaña_AñoMes",
                table: "Objetivos");

            migrationBuilder.DropIndex(
                name: "IX_Cancelaciones_ReservaId_Fecha",
                table: "Cancelaciones");

            migrationBuilder.DropColumn(
                name: "MotivosCancelacion",
                table: "Reservas");

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Reservas",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Reservas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "MetaOcupacion",
                table: "Objetivos",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Objetivos",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Año",
                table: "Objetivos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdCabaña_FechaEntrada_FechaSalida",
                table: "Reservas",
                columns: new[] { "IdCabaña", "FechaEntrada", "FechaSalida" });

            migrationBuilder.CreateIndex(
                name: "IX_Objetivos_Año_Tipo",
                table: "Objetivos",
                columns: new[] { "Año", "Tipo" },
                unique: true,
                filter: "[Tipo] = 0 AND [IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Objetivos_IdCabaña_Año_Mes_Tipo",
                table: "Objetivos",
                columns: new[] { "IdCabaña", "Año", "Mes", "Tipo" },
                unique: true,
                filter: "[Tipo] = 2 AND [IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Objetivos_IdCabaña_Año_Tipo",
                table: "Objetivos",
                columns: new[] { "IdCabaña", "Año", "Tipo" },
                unique: true,
                filter: "[Tipo] = 1 AND [IsActive] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservas_IdCabaña_FechaEntrada_FechaSalida",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Objetivos_Año_Tipo",
                table: "Objetivos");

            migrationBuilder.DropIndex(
                name: "IX_Objetivos_IdCabaña_Año_Mes_Tipo",
                table: "Objetivos");

            migrationBuilder.DropIndex(
                name: "IX_Objetivos_IdCabaña_Año_Tipo",
                table: "Objetivos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Reservas");

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Reservas",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<int>(
                name: "MotivosCancelacion",
                table: "Reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "MetaOcupacion",
                table: "Objetivos",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Objetivos",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Año",
                table: "Objetivos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_IdCabaña",
                table: "Reservas",
                column: "IdCabaña");

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

            migrationBuilder.CreateIndex(
                name: "IX_Cancelaciones_ReservaId_Fecha",
                table: "Cancelaciones",
                columns: new[] { "ReservaId", "Fecha" });
        }
    }
}
