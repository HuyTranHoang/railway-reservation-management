using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class UpdateEntityCarriageAndType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfCompartments",
                table: "Carriages");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCompartments",
                table: "CarriageTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfCompartments",
                table: "CarriageTypes");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCompartments",
                table: "Carriages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
