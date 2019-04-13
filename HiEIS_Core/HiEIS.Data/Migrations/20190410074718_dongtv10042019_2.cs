using Microsoft.EntityFrameworkCore.Migrations;

namespace HiEIS.Data.Migrations
{
    public partial class dongtv10042019_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaxNo",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TaxNo",
                table: "Customers",
                column: "TaxNo",
                unique: true,
                filter: "[TaxNo] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_TaxNo",
                table: "Customers");

            migrationBuilder.AlterColumn<string>(
                name: "TaxNo",
                table: "Customers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
