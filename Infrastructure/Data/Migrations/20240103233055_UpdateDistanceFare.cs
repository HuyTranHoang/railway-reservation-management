using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class UpdateDistanceFare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistanceFares_TrainStations_ArrivalStationId",
                table: "DistanceFares");

            migrationBuilder.DropForeignKey(
                name: "FK_DistanceFares_TrainStations_DepartureStationId",
                table: "DistanceFares");

            migrationBuilder.DropIndex(
                name: "IX_DistanceFares_ArrivalStationId",
                table: "DistanceFares");

            migrationBuilder.DropIndex(
                name: "IX_DistanceFares_DepartureStationId",
                table: "DistanceFares");

            migrationBuilder.DropColumn(
                name: "ArrivalStationId",
                table: "DistanceFares");

            migrationBuilder.DropColumn(
                name: "DepartureStationId",
                table: "DistanceFares");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArrivalStationId",
                table: "DistanceFares",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartureStationId",
                table: "DistanceFares",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DistanceFares_ArrivalStationId",
                table: "DistanceFares",
                column: "ArrivalStationId");

            migrationBuilder.CreateIndex(
                name: "IX_DistanceFares_DepartureStationId",
                table: "DistanceFares",
                column: "DepartureStationId");

            migrationBuilder.AddForeignKey(
                name: "FK_DistanceFares_TrainStations_ArrivalStationId",
                table: "DistanceFares",
                column: "ArrivalStationId",
                principalTable: "TrainStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DistanceFares_TrainStations_DepartureStationId",
                table: "DistanceFares",
                column: "DepartureStationId",
                principalTable: "TrainStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
