using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QardlessAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "PhoneMobileVerified",
                table: "Admins",
                newName: "ContactNumberVerified");

            migrationBuilder.RenameColumn(
                name: "PhoneMobile",
                table: "Admins",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Admins",
                newName: "ContactNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Admins",
                newName: "PhoneMobile");

            migrationBuilder.RenameColumn(
                name: "ContactNumberVerified",
                table: "Admins",
                newName: "PhoneMobileVerified");

            migrationBuilder.RenameColumn(
                name: "ContactNumber",
                table: "Admins",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
