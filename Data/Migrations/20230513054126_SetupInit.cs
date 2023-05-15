using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sistema_Organizacion.Data.Migrations
{
    public partial class SetupInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Empleado_Habilidad");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleado",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(nullable: true),
                    Cedula = table.Column<string>(nullable: true),
                    Correo = table.Column<string>(nullable: true),
                    FechaNacimiento = table.Column<DateTime>(nullable: true),
                    FechaIngreso = table.Column<DateTime>(nullable: false),
                    IdJefe = table.Column<int>(nullable: true),
                    IdArea = table.Column<int>(nullable: false),
                    Foto = table.Column<string>(nullable: true),
                    IdAreaNavigationId = table.Column<int>(nullable: true),
                    IdJefeNavigationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empleado_Area_IdAreaNavigationId",
                        column: x => x.IdAreaNavigationId,
                        principalTable: "Area",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Empleado_Empleado_IdJefeNavigationId",
                        column: x => x.IdJefeNavigationId,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmpleadoHabilidad",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmpleado = table.Column<int>(nullable: false),
                    NombreHabilidad = table.Column<string>(nullable: true),
                    IdEmpleadoNavigationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpleadoHabilidad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpleadoHabilidad_Empleado_IdEmpleadoNavigationId",
                        column: x => x.IdEmpleadoNavigationId,
                        principalTable: "Empleado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_IdAreaNavigationId",
                table: "Empleado",
                column: "IdAreaNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleado_IdJefeNavigationId",
                table: "Empleado",
                column: "IdJefeNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadoHabilidad_IdEmpleadoNavigationId",
                table: "EmpleadoHabilidad",
                column: "IdEmpleadoNavigationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpleadoHabilidad");

            migrationBuilder.DropTable(
                name: "Empleado");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleado_Habilidad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEmpleado = table.Column<int>(type: "int", nullable: false),
                    NombreHabilidad = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleado_Habilidad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdArea = table.Column<int>(type: "int", nullable: false),
                    IdJefe = table.Column<int>(type: "int", nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                });
        }
    }
}
