using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class UpdatePayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Passengers_PassengerId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Tickets_TicketId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PassengerId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_TicketId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "PassengerId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PassengerId",
                table: "Payments",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TicketId",
                table: "Payments",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Passengers_PassengerId",
                table: "Payments",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Tickets_TicketId",
                table: "Payments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
