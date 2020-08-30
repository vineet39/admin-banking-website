using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApplication.Migrations
{
    public partial class LoginLock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Locked",
                table: "Login",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutTime",
                table: "Login",
                maxLength: 15,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locked",
                table: "Login");

            migrationBuilder.DropColumn(
                name: "LockoutTime",
                table: "Login");
        }
    }
}
