using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Updatet_tb_Relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_OthersAccounts_ToOtherAccountId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "ToOtherAccountId",
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
                value: "b303afcd-0531-414a-b664-2df2c56d3b5e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "ea171813-5a22-4d11-9827-941bb8a5820b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "7aea79e6-bc9e-4dc8-8f14-4ed219433d00", new DateTime(2024, 7, 21, 11, 43, 27, 815, DateTimeKind.Local).AddTicks(2073), "AQAAAAEAACcQAAAAEJshbJ9WxoQKQ0cCtXx4Rbp7Rzufl+D3WT78db34Wbvt6ZvDPAv6zQAQ+j/STvlgDQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_OthersAccounts_ToOtherAccountId",
                table: "Transactions",
                column: "ToOtherAccountId",
                principalTable: "OthersAccounts",
                principalColumn: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_OthersAccounts_ToOtherAccountId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "ToOtherAccountId",
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
                value: "2bf16128-d3f7-42d6-a032-4a24802b4228");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "269ac2c2-2b0e-43b3-ae89-025786b5673f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "c19945b5-d9eb-4bcd-9019-0c324c6296ce", new DateTime(2024, 7, 20, 16, 44, 44, 832, DateTimeKind.Local).AddTicks(9197), "AQAAAAEAACcQAAAAEDFlbH4TWC6Yd3rW0sCFPnNJS2WM0AxEPHJeUcrHduSC4+cr/OxVmsnyXdduYy3vBQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_OthersAccounts_ToOtherAccountId",
                table: "Transactions",
                column: "ToOtherAccountId",
                principalTable: "OthersAccounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
