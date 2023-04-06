using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelUpMarket.Migrations
{
    /// <inheritdoc />
    public partial class changeCharacterTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Characters",
                newName: "CharacterName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CharacterName",
                table: "Characters",
                newName: "Name");
        }
    }
}
