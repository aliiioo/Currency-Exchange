using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Change_ExchangeRateTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_ExchangeRates_ExchangeRateId",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_ExchangeRateId",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "ExchangeRateId",
                table: "Currencies");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "OthersAccounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "ExchangeRates",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyId",
                table: "ExchangeRates",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyId",
                table: "ExchangeRates",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyId",
                table: "ExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_CurrencyId",
                table: "ExchangeRates");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "OthersAccounts");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "ExchangeRates");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ExchangeRateId",
                table: "Currencies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "356300ed-7268-4a82-badc-10b5358bc516");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "cfbd6235-44ea-4a60-9a7a-c78b73c4c976");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "da2f9797-950d-41f5-926b-20585606d845", new DateTime(2024, 7, 14, 12, 55, 44, 690, DateTimeKind.Local).AddTicks(8726), "AQAAAAEAACcQAAAAECpKrP2Mb3qK5iLeH3WP2cM+iyvD5wPiccItG49nuDfRtPV7HJYJJAp2CtMUnlIyKQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_ExchangeRateId",
                table: "Currencies",
                column: "ExchangeRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_ExchangeRates_ExchangeRateId",
                table: "Currencies",
                column: "ExchangeRateId",
                principalTable: "ExchangeRates",
                principalColumn: "ExchangeRateId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
