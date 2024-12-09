using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetNET.Migrations
{
    /// <inheritdoc />
    public partial class ord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrdonnanceIDOrdonnance",
                table: "Medicaments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamePatient = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MedicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Ordonnances",
                columns: table => new
                {
                    IDOrdonnance = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IDPatient = table.Column<int>(type: "int", nullable: false),
                    IDMedecin = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordonnances", x => x.IDOrdonnance);
                    table.ForeignKey(
                        name: "FK_Ordonnances_Medecins_IDMedecin",
                        column: x => x.IDMedecin,
                        principalTable: "Medecins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ordonnances_Patients_IDPatient",
                        column: x => x.IDPatient,
                        principalTable: "Patients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_OrdonnanceIDOrdonnance",
                table: "Medicaments",
                column: "OrdonnanceIDOrdonnance");

            migrationBuilder.CreateIndex(
                name: "IX_Ordonnances_IDMedecin",
                table: "Ordonnances",
                column: "IDMedecin");

            migrationBuilder.CreateIndex(
                name: "IX_Ordonnances_IDPatient",
                table: "Ordonnances",
                column: "IDPatient");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicaments_Ordonnances_OrdonnanceIDOrdonnance",
                table: "Medicaments",
                column: "OrdonnanceIDOrdonnance",
                principalTable: "Ordonnances",
                principalColumn: "IDOrdonnance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicaments_Ordonnances_OrdonnanceIDOrdonnance",
                table: "Medicaments");

            migrationBuilder.DropTable(
                name: "Ordonnances");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Medicaments_OrdonnanceIDOrdonnance",
                table: "Medicaments");

            migrationBuilder.DropColumn(
                name: "OrdonnanceIDOrdonnance",
                table: "Medicaments");
        }
    }
}
