using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cow_Farm.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToVaccineTypeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VaccineTypes_Name",
                table: "VaccineTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VaccineTypes_Name",
                table: "VaccineTypes");
        }
    }
}
