using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class UpdateCarriage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceCharge",
                table: "Carriages");

            migrationBuilder.AddColumn<int>(
                name: "CarriageTypeId",
                table: "Carriages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Carriages_CarriageTypeId",
                table: "Carriages",
                column: "CarriageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carriages_CarriageTypes_CarriageTypeId",
                table: "Carriages",
                column: "CarriageTypeId",
                principalTable: "CarriageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carriages_CarriageTypes_CarriageTypeId",
                table: "Carriages");

            migrationBuilder.DropIndex(
                name: "IX_Carriages_CarriageTypeId",
                table: "Carriages");

            migrationBuilder.DropColumn(
                name: "CarriageTypeId",
                table: "Carriages");

            migrationBuilder.AddColumn<double>(
                name: "ServiceCharge",
                table: "Carriages",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
