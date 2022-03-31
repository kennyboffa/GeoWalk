using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoWalk_mvc.Migrations
{
    public partial class newcontent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Contents_QuestionAnswerId",
                table: "Answer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answer",
                table: "Answer");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "Answers");

            migrationBuilder.RenameIndex(
                name: "IX_Answer_QuestionAnswerId",
                table: "Answers",
                newName: "IX_Answers_QuestionAnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answers",
                table: "Answers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Contents_QuestionAnswerId",
                table: "Answers",
                column: "QuestionAnswerId",
                principalTable: "Contents",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Contents_QuestionAnswerId",
                table: "Answers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answers",
                table: "Answers");

            migrationBuilder.RenameTable(
                name: "Answers",
                newName: "Answer");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_QuestionAnswerId",
                table: "Answer",
                newName: "IX_Answer_QuestionAnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answer",
                table: "Answer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Contents_QuestionAnswerId",
                table: "Answer",
                column: "QuestionAnswerId",
                principalTable: "Contents",
                principalColumn: "Id");
        }
    }
}
