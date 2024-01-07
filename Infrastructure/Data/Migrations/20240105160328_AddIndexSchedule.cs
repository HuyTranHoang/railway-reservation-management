using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class AddIndexSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_TrainId",
                table: "Schedules");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalDate",
                table: "Schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DepartureDate",
                table: "Schedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TrainId_DepartureDate_DepartureTime",
                table: "Schedules",
                columns: new[] { "TrainId", "DepartureDate", "DepartureTime" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Schedules_TrainId_DepartureDate_DepartureTime",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "ArrivalDate",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "DepartureDate",
                table: "Schedules");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TrainId",
                table: "Schedules",
                column: "TrainId");
        }
    }
}
