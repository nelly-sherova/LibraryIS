using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryIS.Migrations
{
    /// <inheritdoc />
    public partial class addIsFramedIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFramed",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFramed",
                table: "Books");
        }
    }
}
