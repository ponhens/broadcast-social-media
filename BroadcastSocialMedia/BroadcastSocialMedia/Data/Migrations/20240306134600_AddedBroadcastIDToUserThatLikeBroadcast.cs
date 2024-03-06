using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BroadcastSocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedBroadcastIDToUserThatLikeBroadcast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersThatLikeBroadcast_Broadcasts_BroadcastId",
                table: "UsersThatLikeBroadcast");

            migrationBuilder.RenameColumn(
                name: "BroadcastId",
                table: "UsersThatLikeBroadcast",
                newName: "BroadcastID");

            migrationBuilder.RenameIndex(
                name: "IX_UsersThatLikeBroadcast_BroadcastId",
                table: "UsersThatLikeBroadcast",
                newName: "IX_UsersThatLikeBroadcast_BroadcastID");

            migrationBuilder.AlterColumn<int>(
                name: "BroadcastID",
                table: "UsersThatLikeBroadcast",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersThatLikeBroadcast_Broadcasts_BroadcastID",
                table: "UsersThatLikeBroadcast",
                column: "BroadcastID",
                principalTable: "Broadcasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersThatLikeBroadcast_Broadcasts_BroadcastID",
                table: "UsersThatLikeBroadcast");

            migrationBuilder.RenameColumn(
                name: "BroadcastID",
                table: "UsersThatLikeBroadcast",
                newName: "BroadcastId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersThatLikeBroadcast_BroadcastID",
                table: "UsersThatLikeBroadcast",
                newName: "IX_UsersThatLikeBroadcast_BroadcastId");

            migrationBuilder.AlterColumn<int>(
                name: "BroadcastId",
                table: "UsersThatLikeBroadcast",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersThatLikeBroadcast_Broadcasts_BroadcastId",
                table: "UsersThatLikeBroadcast",
                column: "BroadcastId",
                principalTable: "Broadcasts",
                principalColumn: "Id");
        }
    }
}
