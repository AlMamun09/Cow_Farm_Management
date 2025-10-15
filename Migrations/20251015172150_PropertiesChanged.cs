using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cow_Farm.Migrations
{
    /// <inheritdoc />
    public partial class PropertiesChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "VaccineTypes",
                newName: "VaccineManufacturer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VaccineManufacturer",
                table: "VaccineTypes",
                newName: "CompanyName");
        }
    }
}
