using Microsoft.EntityFrameworkCore.Migrations;

namespace my_shoppinglist_api.Migrations
{
    public partial class AddedOwnerConstrains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "ShoppingGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingGroup_OwnerId",
                table: "ShoppingGroup",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingGroup_User_OwnerId",
                table: "ShoppingGroup",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingGroup_User_OwnerId",
                table: "ShoppingGroup");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingGroup_OwnerId",
                table: "ShoppingGroup");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ShoppingGroup");
        }
    }
}
