using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update_AccountTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "2bf16128-d3f7-42d6-a032-4a24802b4228");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "269ac2c2-2b0e-43b3-ae89-025786b5673f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "c19945b5-d9eb-4bcd-9019-0c324c6296ce", new DateTime(2024, 7, 20, 16, 44, 44, 832, DateTimeKind.Local).AddTicks(9197), "AQAAAAEAACcQAAAAEDFlbH4TWC6Yd3rW0sCFPnNJS2WM0AxEPHJeUcrHduSC4+cr/OxVmsnyXdduYy3vBQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Accounts");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "e829025d-e090-4ab0-b292-a2c689da2c61");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "a610a84b-7579-419c-87c0-e70a8a64ee12");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "29191aca-5d6e-44d2-af3e-cf3f2aecb203", new DateTime(2024, 7, 20, 10, 36, 29, 523, DateTimeKind.Local).AddTicks(3865), "AQAAAAEAACcQAAAAEIGRb2cVuFsEl1xj6X3wk/R1Op+5ZYj1atNGSpAJqxUjkTgFN/idsukS6WG0iOb0DA==" });
        }
    }
}
