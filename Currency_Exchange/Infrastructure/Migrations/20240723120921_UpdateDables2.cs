using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateDables2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "7f119109-ae69-4678-a395-b220c3e4bcac");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "8e64f053-4a6b-4b7a-bfad-77457047b662");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "98d1d03f-b032-4c4e-98b1-358179517a44", new DateTime(2024, 7, 23, 15, 39, 19, 412, DateTimeKind.Local).AddTicks(7383), "AQAAAAEAACcQAAAAEI1Gw8XbXpyGDPzH3GAhBJjqQR3GADTesKIA34rfph7vV4wWwt85afgWrUSh4yYRjg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "b7e4f680-1dc7-44b4-9afc-45d180695da2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "d73d7a43-33d5-47c6-ac11-41bf3a277863");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "fba1466c-2fea-410f-8438-609f21cb75bb", new DateTime(2024, 7, 23, 15, 37, 4, 16, DateTimeKind.Local).AddTicks(5129), "AQAAAAEAACcQAAAAEAVbdFeKZqkaU2b81rKkhKvB8pYwkNUX5jJjFT/W210ViK6aXIeoob/xADWidYSw5Q==" });
        }
    }
}
