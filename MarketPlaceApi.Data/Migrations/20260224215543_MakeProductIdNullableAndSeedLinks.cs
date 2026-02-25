using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlaceApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeProductIdNullableAndSeedLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Links",
                table: "Links");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "Links",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Links",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Links",
                columns: new[] { "LinkId", "Image", "IsActive", "ProductId", "Url" },
                values: new object[] { 1, "", true, null, "https://misitio.com/imagen1.jpg" });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Links",
                table: "Links",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Links",
                table: "Links");

            migrationBuilder.DeleteData(
                table: "Links",
                keyColumn: "LinkId",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Links");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "Links",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Links",
                table: "Links",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
