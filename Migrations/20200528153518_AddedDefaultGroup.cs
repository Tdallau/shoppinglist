using Microsoft.EntityFrameworkCore.Migrations;

namespace my_shoppinglist_api.Migrations
{
    public partial class AddedDefaultGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Default",
                table: "ShoppingGroupUser",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Default",
                table: "ShoppingGroupUser");
        }
    }
}
