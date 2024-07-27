using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update_OtherAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "OthersAccounts");

            migrationBuilder.AddColumn<int>(
                name: "RealAccountId",
                table: "OthersAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "f37d0f34-75ee-4b30-91cb-ac7f6ad9e2d2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "5d2a6ae6-7375-4609-a492-42468773e8ba");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "e82649f1-5a1c-4701-a90d-2971231a90eb", new DateTime(2024, 7, 27, 11, 24, 7, 891, DateTimeKind.Local).AddTicks(2944), "AQAAAAEAACcQAAAAEJI1c6tdznMw7MB7CzcttUUwzyrvld2Xj5QYsycXspDx30Qmi0xCqjdsU/VW3i4FJQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RealAccountId",
                table: "OthersAccounts");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "OthersAccounts",
                type: "decimal(18,2)",
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
    }
}
