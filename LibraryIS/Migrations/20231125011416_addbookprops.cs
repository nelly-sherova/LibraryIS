using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryIS.Migrations
{
    /// <inheritdoc />
    public partial class addbookprops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Binding",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CountOfPages",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Binding",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CountOfPages",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "Books");
        }
    }
}
