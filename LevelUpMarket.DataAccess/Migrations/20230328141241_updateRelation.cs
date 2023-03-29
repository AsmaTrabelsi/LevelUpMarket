using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelUpMarket.Migrations
{
    /// <inheritdoc />
    public partial class updateRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genders_Games_GameId",
                table: "Genders");

            migrationBuilder.DropForeignKey(
                name: "FK_Subtitles_Games_GameId",
                table: "Subtitles");

            migrationBuilder.DropForeignKey(
                name: "FK_VoiceLanguages_Games_GameId",
                table: "VoiceLanguages");

            migrationBuilder.DropIndex(
                name: "IX_VoiceLanguages_GameId",
                table: "VoiceLanguages");

            migrationBuilder.DropIndex(
                name: "IX_Subtitles_GameId",
                table: "Subtitles");

            migrationBuilder.DropIndex(
                name: "IX_Genders_GameId",
                table: "Genders");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "VoiceLanguages");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Subtitles");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Genders");

            migrationBuilder.CreateTable(
                name: "GameGenders",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    GendersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenders", x => new { x.GamesId, x.GendersId });
                    table.ForeignKey(
                        name: "FK_GameGenders_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGenders_Genders_GendersId",
                        column: x => x.GendersId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameSubtitle",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    SubtitlesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSubtitle", x => new { x.GamesId, x.SubtitlesId });
                    table.ForeignKey(
                        name: "FK_GameSubtitle_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameSubtitle_Subtitles_SubtitlesId",
                        column: x => x.SubtitlesId,
                        principalTable: "Subtitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameVoiceLanguages",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    VoiceLanguagesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameVoiceLanguages", x => new { x.GamesId, x.VoiceLanguagesId });
                    table.ForeignKey(
                        name: "FK_GameVoiceLanguages_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameVoiceLanguages_VoiceLanguages_VoiceLanguagesId",
                        column: x => x.VoiceLanguagesId,
                        principalTable: "VoiceLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameGenders_GendersId",
                table: "GameGenders",
                column: "GendersId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSubtitle_SubtitlesId",
                table: "GameSubtitle",
                column: "SubtitlesId");

            migrationBuilder.CreateIndex(
                name: "IX_GameVoiceLanguages_VoiceLanguagesId",
                table: "GameVoiceLanguages",
                column: "VoiceLanguagesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGenders");

            migrationBuilder.DropTable(
                name: "GameSubtitle");

            migrationBuilder.DropTable(
                name: "GameVoiceLanguages");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "VoiceLanguages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Subtitles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Genders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VoiceLanguages_GameId",
                table: "VoiceLanguages",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Subtitles_GameId",
                table: "Subtitles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Genders_GameId",
                table: "Genders",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genders_Games_GameId",
                table: "Genders",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subtitles_Games_GameId",
                table: "Subtitles",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoiceLanguages_Games_GameId",
                table: "VoiceLanguages",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
