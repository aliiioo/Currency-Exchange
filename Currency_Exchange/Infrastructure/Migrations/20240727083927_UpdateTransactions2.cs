using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateTransactions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UserBalance",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "a1c8bb67-2b4d-411c-9495-b9413879380c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "f10f2a83-7fb0-46ce-810a-22e501be96ae");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "e6dee04c-be6c-45f2-b0c8-0f5c2e0d0b90", new DateTime(2024, 7, 27, 12, 9, 25, 594, DateTimeKind.Local).AddTicks(9853), "AQAAAAEAACcQAAAAEGuABTpCcnZHouOAnxzKrCnRFWath2Ybv7AtJxIWB6SRxcV1AJLz4QVIVYIvYtqVTQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserBalance",
                table: "Transactions");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "cf9244c6-44ac-499d-9ca6-82944d233583");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "c062ffd2-69f9-43c6-8a57-377af62cdee0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "8b8a57f2-ff52-4860-bb86-58df062bf701", new DateTime(2024, 7, 27, 12, 5, 40, 753, DateTimeKind.Local).AddTicks(2274), "AQAAAAEAACcQAAAAEJk6Yfiv1FuYcypkES58EbZtTGoouC8z/GUy9uVYOVJ+la3URZ1MStZVMfIvY9DKDQ==" });
        }
    }
}
