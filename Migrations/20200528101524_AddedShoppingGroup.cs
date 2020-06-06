using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace my_shoppinglist_api.Migrations
{
    public partial class AddedShoppingGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingGroupUser",
                columns: table => new
                {
                    ShoppingGroupId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingGroupUser", x => new { x.UserId, x.ShoppingGroupId });
                    table.ForeignKey(
                        name: "FK_ShoppingGroupUser_ShoppingGroup_ShoppingGroupId",
                        column: x => x.ShoppingGroupId,
                        principalTable: "ShoppingGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingGroupUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingGroupUser_ShoppingGroupId",
                table: "ShoppingGroupUser",
                column: "ShoppingGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingGroupUser");

            migrationBuilder.DropTable(
                name: "ShoppingGroup");
        }
    }
}
