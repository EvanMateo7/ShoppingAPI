using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingAPI.API.Migrations
{
    public partial class AddTimestampProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Products",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Products");
        }
    }
}
