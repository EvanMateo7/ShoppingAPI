using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingAPI.API.Migrations
{
    public partial class ChangeProductTimestamptoRowVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "Products",
                newName: "RowVersion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "Products",
                newName: "Timestamp");
        }
    }
}
