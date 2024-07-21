using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Updatet_tb_Relations_Trnsactions4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                value: "0b23ae38-2841-4b62-9989-e26315a3f35b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "7c71f85b-8514-478d-9ef3-36dd6d340060");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash" },
                values: new object[] { "47e7cf9e-adcf-45a4-ba31-87450377f70b", new DateTime(2024, 7, 21, 15, 52, 31, 543, DateTimeKind.Local).AddTicks(8878), "AQAAAAEAACcQAAAAEOOwz2UcN5rbbcMWUCYFdZgJi4Vxap+/mTRjYAb/D+/KyMp4ptRDt/TWlfk+DEV59A==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
