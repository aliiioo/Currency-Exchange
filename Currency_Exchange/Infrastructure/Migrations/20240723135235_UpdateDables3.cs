using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateDables3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CartNumber",
                table: "OthersAccounts",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CartNumber",
                table: "Accounts",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "Accounts",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "e74b70ba-23d0-4d61-b7c2-aa231d25edf6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "87c96ba5-11e2-4165-b86e-495009490229");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "ab580355-4109-4e52-9508-936774163247", new DateTime(2024, 7, 23, 17, 22, 33, 899, DateTimeKind.Local).AddTicks(2821), "AQAAAAEAACcQAAAAEBKmBwGN90Cm9TXE4aDnpFkVe0rh7rmLO5xVgxAFuvqr3OTAEJ13xtGB2Qw8yGIU3w==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CartNumber",
                table: "OthersAccounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "CartNumber",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "7f119109-ae69-4678-a395-b220c3e4bcac");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "8e64f053-4a6b-4b7a-bfad-77457047b662");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "98d1d03f-b032-4c4e-98b1-358179517a44", new DateTime(2024, 7, 23, 15, 39, 19, 412, DateTimeKind.Local).AddTicks(7383), "AQAAAAEAACcQAAAAEI1Gw8XbXpyGDPzH3GAhBJjqQR3GADTesKIA34rfph7vV4wWwt85afgWrUSh4yYRjg==" });
        }
    }
}
