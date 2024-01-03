using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class AddCancellationRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CancellationRuleId",
                table: "Cancellation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CancellationRules",
                columns: table => new
                {
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartureDateDifference = table.Column<int>(type: "int", nullable: false),
                    Fee = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationRules", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cancellation_CancellationRuleId",
                table: "Cancellation",
                column: "CancellationRuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cancellation_CancellationRules_CancellationRuleId",
                table: "Cancellation",
                column: "CancellationRuleId",
                principalTable: "CancellationRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cancellation_CancellationRules_CancellationRuleId",
                table: "Cancellation");

            migrationBuilder.DropTable(
                name: "CancellationRules");

            migrationBuilder.DropIndex(
                name: "IX_Cancellation_CancellationRuleId",
                table: "Cancellation");

            migrationBuilder.DropColumn(
                name: "CancellationRuleId",
                table: "Cancellation");
        }
    }
}
