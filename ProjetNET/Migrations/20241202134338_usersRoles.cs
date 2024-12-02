using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjetNET.Migrations
{
    /// <inheritdoc />
    public partial class usersRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a77d522-c050-4e11-87f3-459e680a6fee", "d1fd2b2b-ca7e-4c1d-9220-84aa9ecb902d", "Pharmacien", "pharmacien" },
                    { "51cdcee7-99c2-4aeb-8c08-3dd0e8e516f0", "e4dbb981-1ee6-4cca-a478-f370f74042bb", "Admin", "admin" },
                    { "92fbdedd-d07a-4709-8dbf-c144926b8f33", "2cf74f64-5079-41f4-9949-3485473c0444", "Medecin", "medecin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a77d522-c050-4e11-87f3-459e680a6fee");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "51cdcee7-99c2-4aeb-8c08-3dd0e8e516f0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92fbdedd-d07a-4709-8dbf-c144926b8f33");
        }
    }
}
