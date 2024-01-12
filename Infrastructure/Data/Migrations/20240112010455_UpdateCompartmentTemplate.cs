using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    public partial class UpdateCompartmentTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CompartmentTemplates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("Relational:ColumnOrder", 998);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CompartmentTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false)
                .Annotation("Relational:ColumnOrder", 997);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CompartmentTemplates",
                type: "datetime2",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 999);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CompartmentTemplates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CompartmentTemplates");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CompartmentTemplates");
        }
    }
}
