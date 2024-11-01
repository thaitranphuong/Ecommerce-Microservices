using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryService.Migrations
{
    public partial class addpositioninwarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExpired",
                table: "Exports");

            migrationBuilder.AddColumn<int>(
                name: "ExportedQuantity",
                table: "ImportDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "ImportDetails",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExportedQuantity",
                table: "ImportDetails");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "ImportDetails");

            migrationBuilder.AddColumn<bool>(
                name: "IsExpired",
                table: "Exports",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
