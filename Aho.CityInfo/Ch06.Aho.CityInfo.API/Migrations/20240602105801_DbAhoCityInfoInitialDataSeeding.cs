using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ch06.Aho.CityInfo.API.Migrations
{
    /// <inheritdoc />
    public partial class DbAhoCityInfoInitialDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Big Apple!", "New York City" },
                    { 2, "The capital of French gastronomy!", "Lyon" },
                    { 3, "The place of birth of AHO!", "Jijel" }
                });

            migrationBuilder.InsertData(
                table: "PointsOfInterest",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, "The most visited urban park in the United States!", "Central Park" },
                    { 2, 1, "An iconic 102-story skyscraper in Manhattan!", "Empire State Building" },
                    { 3, 2, "A beautiful cathedral (late 19th century) that overlooks the city of Lyon from the Fourvière Hill.", "Basilica Notre-Dame de Fourvière" },
                    { 4, 2, "A museum of natural history, anthropology, societies, and civilizations, housed in a modern-designed building.", "Confluences Museum" },
                    { 5, 3, "Known for its diverse landscapes, including forests, mountains, and beaches.", "Taza National Park" },
                    { 6, 3, "An iconic landmark along the shores of Jijel.", "Phare de Ras Afia" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
