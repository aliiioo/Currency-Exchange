using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DeductedAmount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeductedAmount",
                table: "Transactions");

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
    }
}
