using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelUpMarket.Migrations
{
    /// <inheritdoc />
    public partial class addRelationBetweenGameAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Character_Games_GameId",
                table: "Character");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Developer_DeveloperId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Developer",
                table: "Developer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Character",
                table: "Character");

            migrationBuilder.RenameTable(
                name: "Developer",
                newName: "Developers");

            migrationBuilder.RenameTable(
                name: "Character",
                newName: "Characters");

            migrationBuilder.RenameIndex(
                name: "IX_Character_GameId",
                table: "Characters",
                newName: "IX_Characters_GameId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Developers",
                table: "Developers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Characters",
                table: "Characters",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CategoryId",
                table: "Games",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Games_GameId",
                table: "Characters",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Categories_CategoryId",
                table: "Games",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Developers_DeveloperId",
                table: "Games",
                column: "DeveloperId",
                principalTable: "Developers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Games_GameId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Categories_CategoryId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Developers_DeveloperId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_CategoryId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Developers",
                table: "Developers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Characters",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "Developers",
                newName: "Developer");

            migrationBuilder.RenameTable(
                name: "Characters",
                newName: "Character");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_GameId",
                table: "Character",
                newName: "IX_Character_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Developer",
                table: "Developer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Character",
                table: "Character",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Character_Games_GameId",
                table: "Character",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Developer_DeveloperId",
                table: "Games",
                column: "DeveloperId",
                principalTable: "Developer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
