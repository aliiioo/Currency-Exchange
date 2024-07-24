using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdateDables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwoFactorAuthentications");

            migrationBuilder.DropColumn(
                name: "ConvertedAmount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DailyWithdrawalLimit",
                table: "AspNetUsers");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ConvertedAmount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DailyWithdrawalLimit",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "TwoFactorAuthentications",
                columns: table => new
                {
                    TwoFactorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoFactorAuthentications", x => x.TwoFactorId);
                    table.ForeignKey(
                        name: "FK_TwoFactorAuthentications_AspNetUsers_UserId",
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
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "DailyWithdrawalLimit", "PasswordHash" },
                values: new object[] { "3e4dc946-abf0-44e0-b48c-46252714edcb", new DateTime(2024, 7, 23, 12, 41, 20, 277, DateTimeKind.Local).AddTicks(8440), 10000.00m, "AQAAAAEAACcQAAAAEDlEtfrEPa6tW7a1nFwgUm8UlK5bviXPdaf55dWgcEhnsm9cuLqPRPPx0Y9BcGb0Qg==" });

            migrationBuilder.CreateIndex(
                name: "IX_TwoFactorAuthentications_UserId",
                table: "TwoFactorAuthentications",
                column: "UserId");
        }
    }
}
