using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Database.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Book",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ReaderId",
                table: "Book",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_ReaderId",
                table: "Book",
                column: "ReaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_AspNetUsers_ReaderId",
                table: "Book",
                column: "ReaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_AspNetUsers_ReaderId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_ReaderId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "ReaderId",
                table: "Book");
        }
    }
}
