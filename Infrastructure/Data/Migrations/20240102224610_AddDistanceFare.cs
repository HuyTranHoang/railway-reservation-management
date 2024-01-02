using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class AddDistanceFare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DistanceFares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainId = table.Column<int>(type: "int", nullable: false),
                    DepartureStationId = table.Column<int>(type: "int", nullable: false),
                    ArrivalStationId = table.Column<int>(type: "int", nullable: false),
                    TrainStationId = table.Column<int>(type: "int", nullable: true),
                    Distance = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistanceFares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistanceFares_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistanceFares_TrainStations_TrainStationId",
                        column: x => x.TrainStationId,
                        principalTable: "TrainStations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistanceFares_TrainId",
                table: "DistanceFares",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_DistanceFares_TrainStationId",
                table: "DistanceFares",
                column: "TrainStationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistanceFares");
        }
    }
}
