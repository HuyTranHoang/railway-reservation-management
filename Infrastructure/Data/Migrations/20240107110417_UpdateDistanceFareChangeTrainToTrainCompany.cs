using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class UpdateDistanceFareChangeTrainToTrainCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistanceFares_Trains_TrainId",
                table: "DistanceFares");

            migrationBuilder.RenameColumn(
                name: "TrainId",
                table: "DistanceFares",
                newName: "TrainCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_DistanceFares_TrainId",
                table: "DistanceFares",
                newName: "IX_DistanceFares_TrainCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_DistanceFares_TrainCompanies_TrainCompanyId",
                table: "DistanceFares",
                column: "TrainCompanyId",
                principalTable: "TrainCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistanceFares_TrainCompanies_TrainCompanyId",
                table: "DistanceFares");

            migrationBuilder.RenameColumn(
                name: "TrainCompanyId",
                table: "DistanceFares",
                newName: "TrainId");

            migrationBuilder.RenameIndex(
                name: "IX_DistanceFares_TrainCompanyId",
                table: "DistanceFares",
                newName: "IX_DistanceFares_TrainId");

            migrationBuilder.AddForeignKey(
                name: "FK_DistanceFares_Trains_TrainId",
                table: "DistanceFares",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
