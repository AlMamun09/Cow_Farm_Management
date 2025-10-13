using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cow_Farm.Migrations
{
    /// <inheritdoc />
    public partial class AddParentageToCow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DamId",
                table: "Cows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SireId",
                table: "Cows",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cows_DamId",
                table: "Cows",
                column: "DamId");

            migrationBuilder.CreateIndex(
                name: "IX_Cows_SireId",
                table: "Cows",
                column: "SireId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cows_Cows_DamId",
                table: "Cows",
                column: "DamId",
                principalTable: "Cows",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cows_Cows_SireId",
                table: "Cows",
                column: "SireId",
                principalTable: "Cows",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cows_Cows_DamId",
                table: "Cows");

            migrationBuilder.DropForeignKey(
                name: "FK_Cows_Cows_SireId",
                table: "Cows");

            migrationBuilder.DropIndex(
                name: "IX_Cows_DamId",
                table: "Cows");

            migrationBuilder.DropIndex(
                name: "IX_Cows_SireId",
                table: "Cows");

            migrationBuilder.DropColumn(
                name: "DamId",
                table: "Cows");

            migrationBuilder.DropColumn(
                name: "SireId",
                table: "Cows");
        }
    }
}
