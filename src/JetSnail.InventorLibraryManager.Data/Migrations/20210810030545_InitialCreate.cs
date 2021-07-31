using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JetSnail.InventorLibraryManager.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prototype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FamilyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LibraryId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prototype", x => x.Id);
                    table.UniqueConstraint("AK_Prototype_FamilyId", x => x.FamilyId);
                    table.ForeignKey(
                        name: "FK_Prototype_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Derivative",
                columns: table => new
                {
                    FamilyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LibraryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SynchronizedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrototypeEntityId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Derivative", x => new { x.FamilyId, x.LibraryId });
                    table.ForeignKey(
                        name: "FK_Derivative_Prototype_PrototypeEntityId",
                        column: x => x.PrototypeEntityId,
                        principalTable: "Prototype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Part",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FamilyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PrototypeFamilyEntityId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Part", x => x.Id);
                    table.UniqueConstraint("AK_Part_PartId_FamilyId", x => new { x.PartId, x.FamilyId });
                    table.ForeignKey(
                        name: "FK_Part_Prototype_PrototypeFamilyEntityId",
                        column: x => x.PrototypeFamilyEntityId,
                        principalTable: "Prototype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Derivative_PrototypeEntityId",
                table: "Derivative",
                column: "PrototypeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_ShortName",
                table: "Group",
                column: "ShortName",
                unique: true,
                filter: "[ShortName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Part_PrototypeFamilyEntityId",
                table: "Part",
                column: "PrototypeFamilyEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Prototype_GroupId",
                table: "Prototype",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Derivative");

            migrationBuilder.DropTable(
                name: "Part");

            migrationBuilder.DropTable(
                name: "Prototype");

            migrationBuilder.DropTable(
                name: "Group");
        }
    }
}
