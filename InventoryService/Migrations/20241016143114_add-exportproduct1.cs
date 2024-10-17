using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryService.Migrations
{
    public partial class addexportproduct1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportProduct_Warehouses_WarehouseId",
                table: "ExportProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExportProduct",
                table: "ExportProduct");

            migrationBuilder.DropIndex(
                name: "IX_ExportProduct_WarehouseId",
                table: "ExportProduct");

            migrationBuilder.RenameTable(
                name: "ExportProduct",
                newName: "ExportProducts");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ExportProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExportProducts",
                table: "ExportProducts",
                columns: new[] { "WarehouseId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExportProducts_Warehouses_WarehouseId",
                table: "ExportProducts",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportProducts_Warehouses_WarehouseId",
                table: "ExportProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExportProducts",
                table: "ExportProducts");

            migrationBuilder.RenameTable(
                name: "ExportProducts",
                newName: "ExportProduct");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ExportProduct",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExportProduct",
                table: "ExportProduct",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ExportProduct_WarehouseId",
                table: "ExportProduct",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportProduct_Warehouses_WarehouseId",
                table: "ExportProduct",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
