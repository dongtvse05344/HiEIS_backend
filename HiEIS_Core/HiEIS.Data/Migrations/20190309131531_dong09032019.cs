using Microsoft.EntityFrameworkCore.Migrations;

namespace HiEIS.Data.Migrations
{
    public partial class dong09032019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Invoices",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bank",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Enterprise",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxNo",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tel",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "InvoiceItem",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Total",
                table: "InvoiceItem",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "InvoiceItem",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "UnitPrice",
                table: "InvoiceItem",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Bank",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Enterprise",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TaxNo",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Tel",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "InvoiceItem");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Invoices",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
