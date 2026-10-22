using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawingRegister.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameRevisionNameAddDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Letter",
                table: "Revisions",
                newName: "RevisionLetter");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Revisions",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Revisions");

            migrationBuilder.RenameColumn(
                name: "RevisionLetter",
                table: "Revisions",
                newName: "Letter");
        }
    }
}
