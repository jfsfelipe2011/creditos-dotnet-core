using Microsoft.EntityFrameworkCore.Migrations;

namespace ControleCreditos.Data.Migrations
{
    public partial class AlterClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Client",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Client",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
