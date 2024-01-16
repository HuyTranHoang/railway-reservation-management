using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class UpdateTimeOfSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_TrainId_DepartureDate_DepartureTime_ArrivalStationId_ArrivalDate",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ArrivalDate",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "DepartureDate",
                table: "Schedules",
                newName: "ArrivalTime");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TrainId_DepartureTime_ArrivalStationId_ArrivalTime",
                table: "Schedules",
                columns: new[] { "TrainId", "DepartureTime", "ArrivalStationId", "ArrivalTime" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_TrainId_DepartureTime_ArrivalStationId_ArrivalTime",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "ArrivalTime",
                table: "Schedules",
                newName: "DepartureDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalDate",
                table: "Schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TrainId_DepartureDate_DepartureTime_ArrivalStationId_ArrivalDate",
                table: "Schedules",
                columns: new[] { "TrainId", "DepartureDate", "DepartureTime", "ArrivalStationId", "ArrivalDate" },
                unique: true);
        }
    }
}
