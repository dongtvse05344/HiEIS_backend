using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HiEIS.Data.Migrations
{
    public partial class dong09032019_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Customers_TemplateId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceItem_InvoiceId",
                table: "InvoiceItem");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Invoices");

            migrationBuilder.AddColumn<Guid>(
                name: "TemplateId",
                table: "InvoiceItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "InvoiceItem");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Invoices",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_InvoiceId",
                table: "InvoiceItem",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Customers_TemplateId",
                table: "Invoices",
                column: "TemplateId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
