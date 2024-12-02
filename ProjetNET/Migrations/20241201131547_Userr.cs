using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetNET.Migrations
{
    /// <inheritdoc />
    public partial class Userr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medecins_AspNetUsers_ApplicationUserId2",
                table: "Medecins");

            migrationBuilder.DropForeignKey(
                name: "FK_Pharmaciens_AspNetUsers_ApplicationUserId2",
                table: "Pharmaciens");

            migrationBuilder.DropIndex(
                name: "IX_Pharmaciens_ApplicationUserId2",
                table: "Pharmaciens");

            migrationBuilder.DropIndex(
                name: "IX_Medecins_ApplicationUserId2",
                table: "Medecins");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId2",
                table: "Pharmaciens");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId2",
                table: "Medecins");

            migrationBuilder.RenameColumn(
                name: "licenseNumber",
                table: "Pharmaciens",
                newName: "LicenseNumber");

            migrationBuilder.RenameColumn(
                name: "specialite",
                table: "Medecins",
                newName: "Specialite");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LicenseNumber",
                table: "Pharmaciens",
                newName: "licenseNumber");

            migrationBuilder.RenameColumn(
                name: "Specialite",
                table: "Medecins",
                newName: "specialite");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId2",
                table: "Pharmaciens",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId2",
                table: "Medecins",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pharmaciens_ApplicationUserId2",
                table: "Pharmaciens",
                column: "ApplicationUserId2",
                unique: true,
                filter: "[ApplicationUserId2] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Medecins_ApplicationUserId2",
                table: "Medecins",
                column: "ApplicationUserId2",
                unique: true,
                filter: "[ApplicationUserId2] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Medecins_AspNetUsers_ApplicationUserId2",
                table: "Medecins",
                column: "ApplicationUserId2",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pharmaciens_AspNetUsers_ApplicationUserId2",
                table: "Pharmaciens",
                column: "ApplicationUserId2",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
