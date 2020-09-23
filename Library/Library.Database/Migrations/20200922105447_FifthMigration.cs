using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Database.Migrations
{
    public partial class FifthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_AspNetUsers_ReaderId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_ReaderId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "ReaderId",
                table: "Book");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ReaderId",
                table: "Book",
                type: "bigint",
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
    }
}
