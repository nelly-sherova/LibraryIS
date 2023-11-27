using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryIS.Migrations
{
    /// <inheritdoc />
    public partial class addbookprops3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Collection",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Collection",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Books");
        }
    }
}
