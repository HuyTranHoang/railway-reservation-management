using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class UpdateIndexSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_TrainId_DepartureDate_DepartureTime",
                table: "Schedules");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TrainId_DepartureDate_DepartureTime_ArrivalStationId",
                table: "Schedules",
                columns: new[] { "TrainId", "DepartureDate", "DepartureTime", "ArrivalStationId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_TrainId_DepartureDate_DepartureTime_ArrivalStationId",
                table: "Schedules");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TrainId_DepartureDate_DepartureTime",
                table: "Schedules",
                columns: new[] { "TrainId", "DepartureDate", "DepartureTime" },
                unique: true);
        }
    }
}
