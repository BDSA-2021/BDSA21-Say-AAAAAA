using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SELearning.Infrastructure.Migrations
{
    public partial class RestructureContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Content",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Content_AuthorId",
                table: "Content",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Users_AuthorId",
                table: "Content",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Content_Users_AuthorId",
                table: "Content");

            migrationBuilder.DropIndex(
                name: "IX_Content_AuthorId",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Content",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
