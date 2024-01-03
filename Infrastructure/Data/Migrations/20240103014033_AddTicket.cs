using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class AddTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carriages_Trains_TrainId",
                table: "Carriages");

            migrationBuilder.DropForeignKey(
                name: "FK_Compartments_Carriages_CarriageId",
                table: "Compartments");

            migrationBuilder.DropForeignKey(
                name: "FK_DistanceFares_Trains_TrainId",
                table: "DistanceFares");

            migrationBuilder.DropForeignKey(
                name: "FK_DistanceFares_TrainStations_TrainStationId",
                table: "DistanceFares");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Trains_TrainId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_TrainStations_TrainStationId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Trains_TrainCompanies_TrainCompanyId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_TrainStationId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_DistanceFares_TrainStationId",
                table: "DistanceFares");

            migrationBuilder.DropColumn(
                name: "TrainStationId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "TrainStationId",
                table: "DistanceFares");

            migrationBuilder.AddColumn<string>(
                name: "CardId",
                table: "Passengers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PassengerId = table.Column<int>(type: "int", nullable: false),
                    TrainId = table.Column<int>(type: "int", nullable: false),
                    DistanceFareId = table.Column<int>(type: "int", nullable: false),
                    CarriageId = table.Column<int>(type: "int", nullable: false),
                    SeatId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Carriages_CarriageId",
                        column: x => x.CarriageId,
                        principalTable: "Carriages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_DistanceFares_DistanceFareId",
                        column: x => x.DistanceFareId,
                        principalTable: "DistanceFares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ArrivalStationId",
                table: "Schedules",
                column: "ArrivalStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_DepartureStationId",
                table: "Schedules",
                column: "DepartureStationId");

            migrationBuilder.CreateIndex(
                name: "IX_DistanceFares_ArrivalStationId",
                table: "DistanceFares",
                column: "ArrivalStationId");

            migrationBuilder.CreateIndex(
                name: "IX_DistanceFares_DepartureStationId",
                table: "DistanceFares",
                column: "DepartureStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CarriageId",
                table: "Tickets",
                column: "CarriageId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DistanceFareId",
                table: "Tickets",
                column: "DistanceFareId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PassengerId",
                table: "Tickets",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ScheduleId",
                table: "Tickets",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SeatId",
                table: "Tickets",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TrainId",
                table: "Tickets",
                column: "TrainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carriages_Trains_TrainId",
                table: "Carriages",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Compartments_Carriages_CarriageId",
                table: "Compartments",
                column: "CarriageId",
                principalTable: "Carriages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DistanceFares_Trains_TrainId",
                table: "DistanceFares",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Trains_TrainId",
                table: "Schedules",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_TrainStations_ArrivalStationId",
                table: "Schedules",
                column: "ArrivalStationId",
                principalTable: "TrainStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_TrainStations_DepartureStationId",
                table: "Schedules",
                column: "DepartureStationId",
                principalTable: "TrainStations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_TrainCompanies_TrainCompanyId",
                table: "Trains",
                column: "TrainCompanyId",
                principalTable: "TrainCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carriages_Trains_TrainId",
                table: "Carriages");

            migrationBuilder.DropForeignKey(
                name: "FK_Compartments_Carriages_CarriageId",
                table: "Compartments");

            migrationBuilder.DropForeignKey(
                name: "FK_DistanceFares_Trains_TrainId",
                table: "DistanceFares");

            migrationBuilder.DropForeignKey(
                name: "FK_DistanceFares_TrainStations_ArrivalStationId",
                table: "DistanceFares");

            migrationBuilder.DropForeignKey(
                name: "FK_DistanceFares_TrainStations_DepartureStationId",
                table: "DistanceFares");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Trains_TrainId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_TrainStations_ArrivalStationId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_TrainStations_DepartureStationId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Trains_TrainCompanies_TrainCompanyId",
                table: "Trains");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_ArrivalStationId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_DepartureStationId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_DistanceFares_ArrivalStationId",
                table: "DistanceFares");

            migrationBuilder.DropIndex(
                name: "IX_DistanceFares_DepartureStationId",
                table: "DistanceFares");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Passengers");

            migrationBuilder.AddColumn<int>(
                name: "TrainStationId",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainStationId",
                table: "DistanceFares",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TrainStationId",
                table: "Schedules",
                column: "TrainStationId");

            migrationBuilder.CreateIndex(
                name: "IX_DistanceFares_TrainStationId",
                table: "DistanceFares",
                column: "TrainStationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carriages_Trains_TrainId",
                table: "Carriages",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Compartments_Carriages_CarriageId",
                table: "Compartments",
                column: "CarriageId",
                principalTable: "Carriages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DistanceFares_Trains_TrainId",
                table: "DistanceFares",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DistanceFares_TrainStations_TrainStationId",
                table: "DistanceFares",
                column: "TrainStationId",
                principalTable: "TrainStations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Trains_TrainId",
                table: "Schedules",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_TrainStations_TrainStationId",
                table: "Schedules",
                column: "TrainStationId",
                principalTable: "TrainStations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_TrainCompanies_TrainCompanyId",
                table: "Trains",
                column: "TrainCompanyId",
                principalTable: "TrainCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
