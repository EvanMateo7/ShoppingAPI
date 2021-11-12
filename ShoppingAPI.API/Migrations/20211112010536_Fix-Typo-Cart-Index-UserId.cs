using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingAPI.API.Migrations
{
    public partial class FixTypoCartIndexUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Cartasss_UserId",
                table: "Cart",
                newName: "IX_Cart_UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Cart_UserId",
                table: "Cart",
                newName: "IX_Cartasss_UserId");
        }
    }
}
