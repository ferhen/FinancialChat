using Microsoft.EntityFrameworkCore.Migrations;

namespace FinancialChat.Data.Migrations
{
    public partial class AddChatroomNameConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Chatrooms",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chatrooms_Name",
                table: "Chatrooms",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Chatrooms_Name",
                table: "Chatrooms");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Chatrooms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
