using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductService.Migrations
{
    public partial class updatecommenttable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentUser",
                columns: table => new
                {
                    LikesId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LikesId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentUser", x => new { x.LikesId, x.LikesId1 });
                    table.ForeignKey(
                        name: "FK_CommentUser_Comments_LikesId1",
                        column: x => x.LikesId1,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentUser_Users_LikesId",
                        column: x => x.LikesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CommentUser_LikesId1",
                table: "CommentUser",
                column: "LikesId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentUser");
        }
    }
}
