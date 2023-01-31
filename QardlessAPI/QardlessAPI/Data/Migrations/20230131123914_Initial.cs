using System;
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
                name: "Expires",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "PdfUri",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Certificates",
                newName: "PdfUrl");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "Certificates",
                newName: "CourseTitle");

            migrationBuilder.RenameColumn(
                name: "QrCodeUri",
                table: "Certificates",
                newName: "CertNumber");

            migrationBuilder.AddColumn<DateTime>(
                name: "CourseDate",
                table: "Certificates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseDate",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "PdfUrl",
                table: "Certificates",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "CourseTitle",
                table: "Certificates",
                newName: "SerialNumber");

            migrationBuilder.RenameColumn(
                name: "CertNumber",
                table: "Certificates",
                newName: "QrCodeUri");

            migrationBuilder.AddColumn<bool>(
                name: "Expires",
                table: "Certificates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PdfUri",
                table: "Certificates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
