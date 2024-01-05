using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class RenameNumberOfCompartmentsInCompartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfCompartment",
                table: "Carriages",
                newName: "NumberOfCompartments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfCompartments",
                table: "Carriages",
                newName: "NumberOfCompartment");
        }
    }
}
