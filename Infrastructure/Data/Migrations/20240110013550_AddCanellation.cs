using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class AddCanellation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cancellation_CancellationRules_CancellationRuleId",
                table: "Cancellation");

            migrationBuilder.DropForeignKey(
                name: "FK_Cancellation_Tickets_TicketId",
                table: "Cancellation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cancellation",
                table: "Cancellation");

            migrationBuilder.DropColumn(
                name: "RefundAmount",
                table: "Cancellation");

            migrationBuilder.RenameTable(
                name: "Cancellation",
                newName: "Cancellations");

            migrationBuilder.RenameIndex(
                name: "IX_Cancellation_TicketId",
                table: "Cancellations",
                newName: "IX_Cancellations_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Cancellation_CancellationRuleId",
                table: "Cancellations",
                newName: "IX_Cancellations_CancellationRuleId");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "Cancellations",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cancellations",
                table: "Cancellations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cancellations_CancellationRules_CancellationRuleId",
                table: "Cancellations",
                column: "CancellationRuleId",
                principalTable: "CancellationRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cancellations_Tickets_TicketId",
                table: "Cancellations",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cancellations_CancellationRules_CancellationRuleId",
                table: "Cancellations");

            migrationBuilder.DropForeignKey(
                name: "FK_Cancellations_Tickets_TicketId",
                table: "Cancellations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cancellations",
                table: "Cancellations");

            migrationBuilder.RenameTable(
                name: "Cancellations",
                newName: "Cancellation");

            migrationBuilder.RenameIndex(
                name: "IX_Cancellations_TicketId",
                table: "Cancellation",
                newName: "IX_Cancellation_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Cancellations_CancellationRuleId",
                table: "Cancellation",
                newName: "IX_Cancellation_CancellationRuleId");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "Cancellation",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RefundAmount",
                table: "Cancellation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cancellation",
                table: "Cancellation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cancellation_CancellationRules_CancellationRuleId",
                table: "Cancellation",
                column: "CancellationRuleId",
                principalTable: "CancellationRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cancellation_Tickets_TicketId",
                table: "Cancellation",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
