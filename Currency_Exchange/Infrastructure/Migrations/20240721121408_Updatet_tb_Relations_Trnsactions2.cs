using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Updatet_tb_Relations_Trnsactions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "9e21744e-6d58-4430-9a4f-0166d917628a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "3602a280-f980-44a1-b2d1-4340d8395c5b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "1a744c71-bac2-4890-9ac2-f15cfb047c3c", new DateTime(2024, 7, 21, 15, 44, 6, 851, DateTimeKind.Local).AddTicks(3995), "AQAAAAEAACcQAAAAEHyJmi13lk+dfmdeoMhvyPgORd4Mzg/A+9H0Ena5HlQAvx+iO14wXZSd819cL8i6Nw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
