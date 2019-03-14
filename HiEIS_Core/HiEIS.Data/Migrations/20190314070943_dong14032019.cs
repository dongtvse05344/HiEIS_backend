using Microsoft.EntityFrameworkCore.Migrations;

namespace HiEIS.Data.Migrations
{
    public partial class dong14032019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Llx",
                table: "Templates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Lly",
                table: "Templates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Urx",
                table: "Templates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ury",
                table: "Templates",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Llx",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Lly",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Urx",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "Ury",
                table: "Templates");
        }
    }
}
