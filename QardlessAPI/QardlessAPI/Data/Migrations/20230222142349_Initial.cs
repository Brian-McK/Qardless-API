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
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_EndUsers_EndUserId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_EndUserId",
                table: "Certificates");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_EndUsers_Id",
                table: "Certificates",
                column: "Id",
                principalTable: "EndUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_EndUsers_Id",
                table: "Certificates");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_EndUserId",
                table: "Certificates",
                column: "EndUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_EndUsers_EndUserId",
                table: "Certificates",
                column: "EndUserId",
                principalTable: "EndUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
