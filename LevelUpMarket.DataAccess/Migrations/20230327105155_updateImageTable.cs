using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevelUpMarket.Migrations
{
    /// <inheritdoc />
    public partial class updateImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "Images",
                newName: "Name");

            migrationBuilder.AddColumn<byte[]>(
                name: "Bytes",
                table: "Images",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bytes",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Images",
                newName: "ImageURL");
        }
    }
}
