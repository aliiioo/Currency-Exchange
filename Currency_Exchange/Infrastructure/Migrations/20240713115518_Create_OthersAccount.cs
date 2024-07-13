using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Create_OthersAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "ToOtherAccountId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OthersAccount",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OthersAccount", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_OthersAccount_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ToOtherAccountId",
                table: "Transactions",
                column: "ToOtherAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_OthersAccount_UserId",
                table: "OthersAccount",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_OthersAccount_ToOtherAccountId",
                table: "Transactions",
                column: "ToOtherAccountId",
                principalTable: "OthersAccount",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_OthersAccount_ToOtherAccountId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "OthersAccount");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ToOtherAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ToOtherAccountId",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "7cdf3514-d2dd-4234-8bc5-d988fcaa97fc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "f6c6a517-8dfd-4ecc-ae67-c7eedae0606b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "3b346d5a-291f-4a1e-977d-0f5628650442", new DateTime(2024, 7, 13, 14, 22, 20, 132, DateTimeKind.Local).AddTicks(8081), "AQAAAAEAACcQAAAAEPzZJqGQtRcYSIJ+xg6fLgHEQc1jnGYXSQ9LO+YFfFQ3KEMyW3tC25li/sqB27Wlew==" });
        }
    }
}
