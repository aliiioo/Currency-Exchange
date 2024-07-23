using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class DeletedAcount_Tb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeletedAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompleteTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeletedAccounts_AspNetUsers_UserId",
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

            migrationBuilder.CreateIndex(
                name: "IX_DeletedAccounts_UserId",
                table: "DeletedAccounts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedAccounts");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "ee602aa6-3d18-4ddc-b0fa-0002ceee860a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "732915fd-f982-4345-9a80-839a68f2ffbe");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "8da5ccc0-ad93-48ab-99b2-766b4de464f1", new DateTime(2024, 7, 22, 14, 24, 56, 867, DateTimeKind.Local).AddTicks(4599), "AQAAAAEAACcQAAAAEGy6z7funVa9cTkRu5DCP1qjQ39i9I/hBjilTAvjJUWYQSipHaadQa80VBFDNNtIRA==" });
        }
    }
}
