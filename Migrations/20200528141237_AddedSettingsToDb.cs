using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace my_shoppinglist_api.Migrations
{
    public partial class AddedSettingsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingSetting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShoppingGroupId = table.Column<int>(nullable: false),
                    ShopId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    SortingMethod = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingSetting_Shop_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingSetting_ShoppingGroup_ShoppingGroupId",
                        column: x => x.ShoppingGroupId,
                        principalTable: "ShoppingGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingSetting_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingSetting_ShopId",
                table: "ShoppingSetting",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingSetting_ShoppingGroupId",
                table: "ShoppingSetting",
                column: "ShoppingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingSetting_UserId",
                table: "ShoppingSetting",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingSetting");
        }
    }
}
