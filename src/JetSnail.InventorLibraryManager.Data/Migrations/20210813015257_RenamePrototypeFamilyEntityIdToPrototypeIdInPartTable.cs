using Microsoft.EntityFrameworkCore.Migrations;

namespace JetSnail.InventorLibraryManager.Data.Migrations
{
    public partial class RenamePrototypeFamilyEntityIdToPrototypeIdInPartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
            name: "FK_Part_Prototype_PrototypeFamilyEntityId",
            table: "Part");

            migrationBuilder.RenameColumn(
                name: "PrototypeFamilyEntityId",
                table: "Part",
                newName: "PrototypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Part_PrototypeFamilyEntityId",
                table: "Part",
                newName: "IX_Part_PrototypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Part_Prototype_PrototypeId",
                table: "Part",
                column: "PrototypeId",
                principalTable: "Prototype",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
             name: "FK_Part_Prototype_PrototypeId",
             table: "Part");

            migrationBuilder.RenameColumn(
                name: "PrototypeId",
                table: "Part",
                newName: "PrototypeFamilyEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Part_PrototypeId",
                table: "Part",
                newName: "IX_Part_PrototypeFamilyEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Part_Prototype_PrototypeFamilyEntityId",
                table: "Part",
                column: "PrototypeFamilyEntityId",
                principalTable: "Prototype",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
