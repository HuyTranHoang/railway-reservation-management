using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class AddCarriageTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cancellations_TicketId",
                table: "Cancellations");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CarriageTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "CarriageTemplates",
                columns: table => new
                {
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CarriageTypeId = table.Column<int>(type: "int", nullable: false),
                    NumberOfCompartments = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarriageTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarriageTemplates_CarriageTypes_CarriageTypeId",
                        column: x => x.CarriageTypeId,
                        principalTable: "CarriageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cancellations_TicketId",
                table: "Cancellations",
                column: "TicketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarriageTemplates_CarriageTypeId",
                table: "CarriageTemplates",
                column: "CarriageTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarriageTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Cancellations_TicketId",
                table: "Cancellations");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CarriageTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cancellations_TicketId",
                table: "Cancellations",
                column: "TicketId");
        }
    }
}
