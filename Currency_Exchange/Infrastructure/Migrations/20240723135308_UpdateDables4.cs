using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateDables4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
