using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    public partial class UpdateSessionFocusGroupRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_FocusGroups_FocusGroupFocGrId",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "FocusGroupFocGrId",
                table: "Sessions",
                newName: "FocusGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_FocusGroupFocGrId",
                table: "Sessions",
                newName: "IX_Sessions_FocusGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_FocusGroups_FocusGroupId",
                table: "Sessions",
                column: "FocusGroupId",
                principalTable: "FocusGroups",
                principalColumn: "FocGrId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_FocusGroups_FocusGroupId",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "FocusGroupId",
                table: "Sessions",
                newName: "FocusGroupFocGrId");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_FocusGroupId",
                table: "Sessions",
                newName: "IX_Sessions_FocusGroupFocGrId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_FocusGroups_FocusGroupFocGrId",
                table: "Sessions",
                column: "FocusGroupFocGrId",
                principalTable: "FocusGroups",
                principalColumn: "FocGrId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
