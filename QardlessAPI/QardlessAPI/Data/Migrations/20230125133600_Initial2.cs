using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QardlessAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressCode",
                table: "EndUsers");

            migrationBuilder.DropColumn(
                name: "AddressDetailed",
                table: "EndUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "EndUsers");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "EndUsers");

            migrationBuilder.DropColumn(
                name: "PhoneHome",
                table: "EndUsers");

            migrationBuilder.DropColumn(
                name: "PhoneMobileVerified",
                table: "EndUsers");

            migrationBuilder.RenameColumn(
                name: "PhoneMobile",
                table: "EndUsers",
                newName: "ContactNumber");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "EndUsers",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "EndUsers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "ContactNumber",
                table: "EndUsers",
                newName: "PhoneMobile");

            migrationBuilder.AddColumn<string>(
                name: "AddressCode",
                table: "EndUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressDetailed",
                table: "EndUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "EndUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "EndUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneHome",
                table: "EndUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneMobileVerified",
                table: "EndUsers",
                type: "bit",
                nullable: true);
        }
    }
}
