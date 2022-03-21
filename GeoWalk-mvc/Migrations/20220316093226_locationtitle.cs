using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoWalk_mvc.Migrations
{
    public partial class locationtitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Locations");
        }
    }
}
