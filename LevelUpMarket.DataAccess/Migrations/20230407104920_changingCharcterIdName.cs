using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelUpMarket.Migrations
{
    /// <inheritdoc />
    public partial class changingCharcterIdName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Characters",
                newName: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CharacterId",
                table: "Characters",
                newName: "Id");
        }
    }
}
