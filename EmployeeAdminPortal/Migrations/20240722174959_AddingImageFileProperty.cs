using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAdminPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddingImageFileProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Employees");
        }
    }
}
