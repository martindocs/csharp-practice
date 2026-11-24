using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawingRegister.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeDocumentNumberRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Drawings_DocumentNumber",
                table: "Drawings");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentNumber",
                table: "Drawings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drawings_DocumentNumber",
                table: "Drawings",
                column: "DocumentNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Drawings_DocumentNumber",
                table: "Drawings");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentNumber",
                table: "Drawings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Drawings_DocumentNumber",
                table: "Drawings",
                column: "DocumentNumber",
                unique: true,
                filter: "[DocumentNumber] IS NOT NULL");
        }
    }
}
