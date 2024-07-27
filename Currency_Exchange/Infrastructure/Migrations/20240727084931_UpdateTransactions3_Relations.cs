using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateTransactions3_Relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_OthersAccounts_ToOtherAccountId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "ToOtherAccountId",
                table: "Transactions",
                newName: "OthersAccountAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ToOtherAccountId",
                table: "Transactions",
                newName: "IX_Transactions_OthersAccountAccountId");

            migrationBuilder.AlterColumn<int>(
                name: "ToAccountId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "1f19887e-a7f0-47de-89d2-57823bba9199");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "a5210dac-6d56-4e7f-81fe-e44e85871d7b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "1c9c64b2-43ef-443d-a5ab-fffe65ec5512", new DateTime(2024, 7, 27, 12, 19, 29, 568, DateTimeKind.Local).AddTicks(585), "AQAAAAEAACcQAAAAECchsCkgpEZFCcWUcVIe5lb18hiBU7jF/s4diN3Awk1YvS90rbEZpw+CPPeQNRIR4w==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_OthersAccounts_OthersAccountAccountId",
                table: "Transactions",
                column: "OthersAccountAccountId",
                principalTable: "OthersAccounts",
                principalColumn: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_OthersAccounts_OthersAccountAccountId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "OthersAccountAccountId",
                table: "Transactions",
                newName: "ToOtherAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_OthersAccountAccountId",
                table: "Transactions",
                newName: "IX_Transactions_ToOtherAccountId");

            migrationBuilder.AlterColumn<int>(
                name: "ToAccountId",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_OthersAccounts_ToOtherAccountId",
                table: "Transactions",
                column: "ToOtherAccountId",
                principalTable: "OthersAccounts",
                principalColumn: "AccountId");
        }
    }
}
