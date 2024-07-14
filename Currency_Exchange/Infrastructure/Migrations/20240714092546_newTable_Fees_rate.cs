using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class newTable_Fees_rate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OthersAccount_AspNetUsers_UserId",
                table: "OthersAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_OthersAccount_ToOtherAccountId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "CurrencyConversions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OthersAccount",
                table: "OthersAccount");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "FeePercentage",
                table: "Currencies");

            migrationBuilder.RenameTable(
                name: "OthersAccount",
                newName: "OthersAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_OthersAccount_UserId",
                table: "OthersAccounts",
                newName: "IX_OthersAccounts_UserId");

            migrationBuilder.AddColumn<int>(
                name: "ExchangeRateId",
                table: "Currencies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CartNumber",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "OthersAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CartNumber",
                table: "OthersAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OthersAccounts",
                table: "OthersAccounts",
                column: "AccountId");

            migrationBuilder.CreateTable(
                name: "CurrencyExchangeFees",
                columns: table => new
                {
                    FeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    FromCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartRange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EndRange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyExchangeFees", x => x.FeeId);
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeFees_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyTransformFees",
                columns: table => new
                {
                    FeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    FromCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartRange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EndRange = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTransformFees", x => x.FeeId);
                    table.ForeignKey(
                        name: "FK_CurrencyTransformFees_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeFees_CurrencyId",
                table: "CurrencyExchangeFees",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyTransformFees_CurrencyId",
                table: "CurrencyTransformFees",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_ExchangeRates_ExchangeRateId",
                table: "Currencies",
                column: "ExchangeRateId",
                principalTable: "ExchangeRates",
                principalColumn: "ExchangeRateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OthersAccounts_AspNetUsers_UserId",
                table: "OthersAccounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_OthersAccounts_ToOtherAccountId",
                table: "Transactions",
                column: "ToOtherAccountId",
                principalTable: "OthersAccounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_ExchangeRates_ExchangeRateId",
                table: "Currencies");

            migrationBuilder.DropForeignKey(
                name: "FK_OthersAccounts_AspNetUsers_UserId",
                table: "OthersAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_OthersAccounts_ToOtherAccountId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "CurrencyExchangeFees");

            migrationBuilder.DropTable(
                name: "CurrencyTransformFees");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_ExchangeRateId",
                table: "Currencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OthersAccounts",
                table: "OthersAccounts");

            migrationBuilder.DropColumn(
                name: "ExchangeRateId",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CartNumber",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "OthersAccounts");

            migrationBuilder.DropColumn(
                name: "CartNumber",
                table: "OthersAccounts");

            migrationBuilder.RenameTable(
                name: "OthersAccounts",
                newName: "OthersAccount");

            migrationBuilder.RenameIndex(
                name: "IX_OthersAccounts_UserId",
                table: "OthersAccount",
                newName: "IX_OthersAccount_UserId");

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "Currencies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FeePercentage",
                table: "Currencies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OthersAccount",
                table: "OthersAccount",
                column: "AccountId");

            migrationBuilder.CreateTable(
                name: "CurrencyConversions",
                columns: table => new
                {
                    ConversionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ToCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyConversions", x => x.ConversionId);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "88062efa-edc9-49a9-96b1-3916d5e05d2c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "0aee63d2-6e5a-4f2b-af6b-d656df8f1446");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "a817bfc1-3ac8-4b7d-8598-ee773637d39c", new DateTime(2024, 7, 13, 15, 25, 16, 758, DateTimeKind.Local).AddTicks(3445), "AQAAAAEAACcQAAAAEN0UyckBT1LZhzSVI5Q+EUNwQyE0VfuIZAkbIxcy0nvNFc1XQkYpdEVmAJEVt0p6KQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_OthersAccount_AspNetUsers_UserId",
                table: "OthersAccount",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_OthersAccount_ToOtherAccountId",
                table: "Transactions",
                column: "ToOtherAccountId",
                principalTable: "OthersAccount",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
