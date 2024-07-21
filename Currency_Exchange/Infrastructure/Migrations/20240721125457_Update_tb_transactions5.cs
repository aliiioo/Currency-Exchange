using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update_tb_transactions5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "0b23ae38-2841-4b62-9989-e26315a3f35b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "7c71f85b-8514-478d-9ef3-36dd6d340060");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "47e7cf9e-adcf-45a4-ba31-87450377f70b", new DateTime(2024, 7, 21, 15, 52, 31, 543, DateTimeKind.Local).AddTicks(8878), "AQAAAAEAACcQAAAAEOOwz2UcN5rbbcMWUCYFdZgJi4Vxap+/mTRjYAb/D+/KyMp4ptRDt/TWlfk+DEV59A==" });
        }
    }
}
