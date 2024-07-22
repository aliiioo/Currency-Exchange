using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class QueryFilter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "ee602aa6-3d18-4ddc-b0fa-0002ceee860a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "732915fd-f982-4345-9a80-839a68f2ffbe");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "8da5ccc0-ad93-48ab-99b2-766b4de464f1", new DateTime(2024, 7, 22, 14, 24, 56, 867, DateTimeKind.Local).AddTicks(4599), "AQAAAAEAACcQAAAAEGy6z7funVa9cTkRu5DCP1qjQ39i9I/hBjilTAvjJUWYQSipHaadQa80VBFDNNtIRA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "eb370209-6097-44a7-9b14-a8c30e5e8d71");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "93543648-61b9-4d3f-bbda-b8d1132b38d6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "836d1634-6850-47f4-af45-6a4a0b9224f7", new DateTime(2024, 7, 21, 16, 24, 55, 446, DateTimeKind.Local).AddTicks(5094), "AQAAAAEAACcQAAAAEDn8hj/Wfq7l3DTH8oJRJFxzjfWap3+xgrtJd1IH2M+5xDja777TtmS6Z2SJgbaCqQ==" });
        }
    }
}
