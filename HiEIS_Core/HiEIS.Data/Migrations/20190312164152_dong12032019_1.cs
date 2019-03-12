using Microsoft.EntityFrameworkCore.Migrations;

namespace HiEIS.Data.Migrations
{
    public partial class dong12032019_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LookupCode",
                table: "Invoices",
                newName: "No");

            migrationBuilder.AddColumn<string>(
                name: "LockupCode",
                table: "Invoices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockupCode",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "No",
                table: "Invoices",
                newName: "LookupCode");
        }
    }
}
