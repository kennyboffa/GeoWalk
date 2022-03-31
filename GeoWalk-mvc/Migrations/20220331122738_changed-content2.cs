using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoWalk_mvc.Migrations
{
    public partial class changedcontent2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Contents_questionAnswerId",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "questionAnswerId",
                table: "Answers",
                newName: "QuestionAnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_questionAnswerId",
                table: "Answers",
                newName: "IX_Answers_QuestionAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Contents_QuestionAnswerId",
                table: "Answers",
                column: "QuestionAnswerId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Contents_QuestionAnswerId",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "QuestionAnswerId",
                table: "Answers",
                newName: "questionAnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_QuestionAnswerId",
                table: "Answers",
                newName: "IX_Answers_questionAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Contents_questionAnswerId",
                table: "Answers",
                column: "questionAnswerId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
