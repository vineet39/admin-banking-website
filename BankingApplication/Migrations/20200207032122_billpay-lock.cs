using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApplication.Migrations
{
    public partial class billpaylock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Login_CustomerID",
                table: "Login");

            migrationBuilder.AddColumn<bool>(
                name: "Locked",
                table: "BillPay",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Login_CustomerID",
                table: "Login",
                column: "CustomerID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Login_CustomerID",
                table: "Login");

            migrationBuilder.DropColumn(
                name: "Locked",
                table: "BillPay");

            migrationBuilder.CreateIndex(
                name: "IX_Login_CustomerID",
                table: "Login",
                column: "CustomerID");
        }
    }
}
