using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhysicalPersonDirectory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOnDeleteActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumbers_PhysicalPersons_PhysicalPersonId",
                table: "PhoneNumbers");

            migrationBuilder.DropForeignKey(
                name: "FK_PhysicalPersons_Cities_CityId",
                table: "PhysicalPersons");

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumbers_PhysicalPersons_PhysicalPersonId",
                table: "PhoneNumbers",
                column: "PhysicalPersonId",
                principalTable: "PhysicalPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhysicalPersons_Cities_CityId",
                table: "PhysicalPersons",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumbers_PhysicalPersons_PhysicalPersonId",
                table: "PhoneNumbers");

            migrationBuilder.DropForeignKey(
                name: "FK_PhysicalPersons_Cities_CityId",
                table: "PhysicalPersons");

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumbers_PhysicalPersons_PhysicalPersonId",
                table: "PhoneNumbers",
                column: "PhysicalPersonId",
                principalTable: "PhysicalPersons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PhysicalPersons_Cities_CityId",
                table: "PhysicalPersons",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
