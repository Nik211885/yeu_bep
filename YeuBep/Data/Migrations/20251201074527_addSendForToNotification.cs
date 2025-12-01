using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YeuBep.Data.Migrations
{
    /// <inheritdoc />
    public partial class addSendForToNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SendForUserId",
                table: "Notifications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SendForUserId",
                table: "Notifications",
                column: "SendForUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_SendForUserId",
                table: "Notifications",
                column: "SendForUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_SendForUserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_SendForUserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SendForUserId",
                table: "Notifications");
        }
    }
}
