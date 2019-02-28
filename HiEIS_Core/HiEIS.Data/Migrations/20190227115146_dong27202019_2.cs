using Microsoft.EntityFrameworkCore.Migrations;

namespace HiEIS.Data.Migrations
{
    public partial class dong27202019_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "From",
                table: "Templates",
                newName: "Form");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Form",
                table: "Templates",
                newName: "From");
        }
    }
}
