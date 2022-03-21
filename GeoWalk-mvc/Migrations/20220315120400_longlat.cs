using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoWalk_mvc.Migrations
{
    public partial class longlat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Coordinates",
                table: "Locations",
                newName: "Longitude");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Locations",
                newName: "Coordinates");
        }
    }
}
