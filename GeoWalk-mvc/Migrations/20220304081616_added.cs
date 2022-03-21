using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoWalk_mvc.Migrations
{
    public partial class added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Contents",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Info",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Contents",
                newName: "Content");
        }
    }
}
