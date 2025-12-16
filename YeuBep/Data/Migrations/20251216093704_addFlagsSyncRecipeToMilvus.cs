using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YeuBep.Data.Migrations
{
    /// <inheritdoc />
    public partial class addFlagsSyncRecipeToMilvus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSyncToMilvus",
                table: "Recipes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSyncToMilvus",
                table: "Recipes");
        }
    }
}
