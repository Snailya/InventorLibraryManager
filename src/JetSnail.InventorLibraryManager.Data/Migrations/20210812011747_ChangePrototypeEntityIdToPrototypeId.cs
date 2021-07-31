using Microsoft.EntityFrameworkCore.Migrations;

namespace JetSnail.InventorLibraryManager.Data.Migrations
{
    public partial class ChangePrototypeEntityIdToPrototypeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Derivative_Prototype_PrototypeEntityId",
                table: "Derivative");

            migrationBuilder.RenameColumn(
                name: "PrototypeEntityId",
                table: "Derivative",
                newName: "PrototypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Derivative_PrototypeEntityId",
                table: "Derivative",
                newName: "IX_Derivative_PrototypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Derivative_Prototype_PrototypeId",
                table: "Derivative",
                column: "PrototypeId",
                principalTable: "Prototype",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Derivative_Prototype_PrototypeId",
                table: "Derivative");

            migrationBuilder.RenameColumn(
                name: "PrototypeId",
                table: "Derivative",
                newName: "PrototypeEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Derivative_PrototypeId",
                table: "Derivative",
                newName: "IX_Derivative_PrototypeEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Derivative_Prototype_PrototypeEntityId",
                table: "Derivative",
                column: "PrototypeEntityId",
                principalTable: "Prototype",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
