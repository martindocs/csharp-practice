using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawingRegister.Data.Migrations
{
    /// <inheritdoc />
    public partial class UniqueDrawingAndRevisionLetter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Revisions_DrawingId",
                table: "Revisions");

            migrationBuilder.RenameColumn(
                name: "ReleaseDate",
                table: "Revisions",
                newName: "RevisionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_DrawingId_RevisionLetter",
                table: "Revisions",
                columns: new[] { "DrawingId", "RevisionLetter" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Revisions_DrawingId_RevisionLetter",
                table: "Revisions");

            migrationBuilder.RenameColumn(
                name: "RevisionDate",
                table: "Revisions",
                newName: "ReleaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_DrawingId",
                table: "Revisions",
                column: "DrawingId");
        }
    }
}
