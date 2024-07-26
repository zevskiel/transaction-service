using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace construction_project_management.Migrations
{
    /// <inheritdoc />
    public partial class AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_ProjectCreatorId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectCreatorId",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "ProjectCategory",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectCategory",
                table: "Projects");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectCreatorId",
                table: "Projects",
                column: "ProjectCreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_ProjectCreatorId",
                table: "Projects",
                column: "ProjectCreatorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
