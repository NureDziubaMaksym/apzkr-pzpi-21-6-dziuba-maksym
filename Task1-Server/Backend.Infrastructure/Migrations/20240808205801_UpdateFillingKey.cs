using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    public partial class UpdateFillingKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FillingGroups",
                table: "FillingGroups");

            migrationBuilder.AlterColumn<int>(
                name: "AddGrId",
                table: "FillingGroups",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FillingGroups",
                table: "FillingGroups",
                column: "AddGrId");

            migrationBuilder.CreateIndex(
                name: "IX_FillingGroups_UserId",
                table: "FillingGroups",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FillingGroups",
                table: "FillingGroups");

            migrationBuilder.DropIndex(
                name: "IX_FillingGroups_UserId",
                table: "FillingGroups");

            migrationBuilder.AlterColumn<int>(
                name: "AddGrId",
                table: "FillingGroups",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FillingGroups",
                table: "FillingGroups",
                columns: new[] { "UserId", "FocusId" });
        }
    }
}
