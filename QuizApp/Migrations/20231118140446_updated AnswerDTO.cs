using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Migrations
{
    public partial class updatedAnswerDTO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizResults_Users_Username",
                table: "QuizResults");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "QuizResults",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResults_Users_Username",
                table: "QuizResults",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizResults_Users_Username",
                table: "QuizResults");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "QuizResults",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResults_Users_Username",
                table: "QuizResults",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username");
        }
    }
}
