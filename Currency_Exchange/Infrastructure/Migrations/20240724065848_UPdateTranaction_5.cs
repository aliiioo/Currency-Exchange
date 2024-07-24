using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UPdateTranaction_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Outer",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "AccountName",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<decimal>(
                name: "OutBalance",
                table: "Accounts",
                type: "decimal(18,2)",
                maxLength: 300,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "41e6eba5-5a7a-4515-9a48-9bb1e9633e1e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "57d495e8-1e52-4946-8c27-8c9c91b91aa8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "3eb8322f-399f-4eb9-bdb8-f7655e2a274f", new DateTime(2024, 7, 24, 10, 28, 46, 202, DateTimeKind.Local).AddTicks(4390), "AQAAAAEAACcQAAAAEDyCUH21+izb3Z/CS7LzYa7ktK98nZgWWrRVe/9NT2sePvY7Dd99gnde74B4CWGXHg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Outer",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "OutBalance",
                table: "Accounts");

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
                value: "2263e295-a780-432b-8221-bb3164b42412");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "8cb03b92-0a1c-4d43-913c-287600f73513");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "879a8009-ec58-49ef-80b5-598933568182", new DateTime(2024, 7, 23, 17, 23, 7, 210, DateTimeKind.Local).AddTicks(3085), "AQAAAAEAACcQAAAAEEOhZ/x7lUrXB7JAF/dNjLZXGUKjsii0cg/KfXS7ZeHuPGE90zyurEH88SIoN45Sxg==" });
        }
    }
}
