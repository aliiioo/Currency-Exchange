using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Update_DeletedAcount_Tb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "DeletedAccounts",
                newName: "AccountId");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "DeletedAccounts",
                type: "nvarchar(1200)",
                maxLength: 1200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "DeletedAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "46971782-348a-4db7-b914-db246be690f9");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "9b864b82-4b79-4c67-a8d5-22e7680dfdb2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "3e4dc946-abf0-44e0-b48c-46252714edcb", new DateTime(2024, 7, 23, 12, 41, 20, 277, DateTimeKind.Local).AddTicks(8440), "AQAAAAEAACcQAAAAEDlEtfrEPa6tW7a1nFwgUm8UlK5bviXPdaf55dWgcEhnsm9cuLqPRPPx0Y9BcGb0Qg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "DeletedAccounts");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "DeletedAccounts",
                newName: "TransactionId");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "DeletedAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1200)",
                oldMaxLength: 1200,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "c9e62e42-e883-4bf9-abea-f4c812f137ec");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "42a246b4-4661-4288-9ef6-bef70f00efed");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "fc8b05b2-123f-4557-873f-5a11857245f9", new DateTime(2024, 7, 23, 11, 46, 37, 860, DateTimeKind.Local).AddTicks(1874), "AQAAAAEAACcQAAAAEPDK1NPKqJrPd0+ZlMisP2ITe4JCgYFWnVWP+UcLa4yPr6xJcM+8hC1mRevBZbY4Ug==" });
        }
    }
}
