using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BroadcastSocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToBroadcast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFilenameGUID",
                table: "Broadcasts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFilenameGUID",
                table: "Broadcasts");
        }
    }
}
