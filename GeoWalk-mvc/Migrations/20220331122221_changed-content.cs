using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoWalk_mvc.Migrations
{
    public partial class changedcontent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "questionAnswerId",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Contents_questionAnswerId",
                table: "Answers",
                column: "questionAnswerId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "QuestionAnswerId",
                table: "Answers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Contents_QuestionAnswerId",
                table: "Answers",
                column: "QuestionAnswerId",
                principalTable: "Contents",
                principalColumn: "Id");
        }
    }
}
