using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bakehouse.Infrastructure.Data.Migrations
{
    public partial class AddTableGenericType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenericType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisabledAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericType", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "6931ed6f-c8b4-4ca1-ac67-6c7db75551c2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 2, "4af24f31-bc66-4d35-8328-6a5c949909b4", "User", "USER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp", "UpdatedAt" },
                values: new object[] { "342dd3d1-fb22-4441-b89c-0be4381d4dea", new DateTime(2022, 7, 13, 0, 21, 24, 143, DateTimeKind.Local).AddTicks(7867), "AQAAAAEAACcQAAAAEBa/jCZhi65/2aQOpcRkRgQr0kWyCG8wBrM4QP0uKwHN6STuWvUJVZA6qS9l8a0Ucw==", "d385efef-f9a3-44c4-b2a9-b81156b468e9", new DateTime(2022, 7, 13, 0, 21, 24, 145, DateTimeKind.Local).AddTicks(6176) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenericType");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "de343f3e-9870-422e-ab17-092fefa1c73b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp", "UpdatedAt" },
                values: new object[] { "3a2b9afc-e2ac-4d2c-a138-296c063d50a6", new DateTime(2022, 6, 11, 2, 40, 51, 766, DateTimeKind.Local).AddTicks(6275), "AQAAAAEAACcQAAAAEExJcC0SfxX4qkPnTSSfPOOMlLvgJJG87LNRK7M7dX5FCi7qOSg9vVAfHW37zmuyNQ==", "87e0be75-08e2-4d7c-b8ad-efb1b6b709c6", new DateTime(2022, 6, 11, 2, 40, 51, 768, DateTimeKind.Local).AddTicks(6647) });
        }
    }
}
