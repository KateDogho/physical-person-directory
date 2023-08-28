using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhysicalPersonDirectory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOnDeleteActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelatedPhysicalPersons_PhysicalPersons_TargetPersonId",
                table: "RelatedPhysicalPersons");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedPhysicalPersons_PhysicalPersons_TargetPersonId",
                table: "RelatedPhysicalPersons",
                column: "TargetPersonId",
                principalTable: "PhysicalPersons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelatedPhysicalPersons_PhysicalPersons_TargetPersonId",
                table: "RelatedPhysicalPersons");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedPhysicalPersons_PhysicalPersons_TargetPersonId",
                table: "RelatedPhysicalPersons",
                column: "TargetPersonId",
                principalTable: "PhysicalPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
