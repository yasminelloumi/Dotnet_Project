using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetNET.Migrations
{
    /// <inheritdoc />
    public partial class fiin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientID",
                table: "Medicaments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_PatientID",
                table: "Medicaments",
                column: "PatientID");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicaments_Patients_PatientID",
                table: "Medicaments",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicaments_Patients_PatientID",
                table: "Medicaments");

            migrationBuilder.DropIndex(
                name: "IX_Medicaments_PatientID",
                table: "Medicaments");

            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "Medicaments");
        }
    }
}
