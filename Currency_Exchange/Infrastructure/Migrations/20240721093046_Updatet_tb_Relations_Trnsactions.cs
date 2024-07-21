using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Updatet_tb_Relations_Trnsactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_ApplicationUserId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Transactions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ApplicationUserId",
                table: "Transactions",
                newName: "IX_Transactions_UserId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "4d401c7f-5398-4be2-97d4-f78e10e44a3f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "1c87a7ea-862a-45b8-8aa5-9649228badc0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "ab2daeb5-4522-4008-8d5b-df7e5eebf145", new DateTime(2024, 7, 21, 13, 0, 43, 817, DateTimeKind.Local).AddTicks(8615), "AQAAAAEAACcQAAAAEH7FLeXUJD0MGLfs8zilCXp0cpoZ2xUQ4K0Lt0gMeQGt8r7dUfGjrorH3CY5ma99Cw==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_UserId",
                table: "Transactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_UserId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Transactions",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                newName: "IX_Transactions_ApplicationUserId");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "b303afcd-0531-414a-b664-2df2c56d3b5e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "ea171813-5a22-4d11-9827-941bb8a5820b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "7aea79e6-bc9e-4dc8-8f14-4ed219433d00", new DateTime(2024, 7, 21, 11, 43, 27, 815, DateTimeKind.Local).AddTicks(2073), "AQAAAAEAACcQAAAAEJshbJ9WxoQKQ0cCtXx4Rbp7Rzufl+D3WT78db34Wbvt6ZvDPAv6zQAQ+j/STvlgDQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_ApplicationUserId",
                table: "Transactions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
