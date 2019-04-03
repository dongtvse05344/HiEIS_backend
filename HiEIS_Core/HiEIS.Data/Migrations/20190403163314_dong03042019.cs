using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiEIS.Data.Migrations
{
    public partial class dong03042019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Invoices",
                nullable: false,
                defaultValue: new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Invoices");
        }
    }
}
