using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class MoveNumberOfSeatsFromCompartmentToCarriageType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfSeats",
                table: "Compartments");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSeats",
                table: "CarriageTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfSeats",
                table: "CarriageTypes");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSeats",
                table: "Compartments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
