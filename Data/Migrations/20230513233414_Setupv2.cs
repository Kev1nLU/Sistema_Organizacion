using Microsoft.EntityFrameworkCore.Migrations;

namespace Sistema_Organizacion.Data.Migrations
{
    public partial class Setupv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdJefe",
                table: "Empleado",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdJefe",
                table: "Empleado",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
