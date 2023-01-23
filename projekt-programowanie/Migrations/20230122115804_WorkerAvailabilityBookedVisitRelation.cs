using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projektprogramowanie.Migrations
{
    /// <inheritdoc />
    public partial class WorkerAvailabilityBookedVisitRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfilePhotoPath",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "WorkerAvailabilityId",
                table: "BookedVisits",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookedVisits_WorkerAvailabilityId",
                table: "BookedVisits",
                column: "WorkerAvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedVisits_WorkersAvailabilities_WorkerAvailabilityId",
                table: "BookedVisits",
                column: "WorkerAvailabilityId",
                principalTable: "WorkersAvailabilities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedVisits_WorkersAvailabilities_WorkerAvailabilityId",
                table: "BookedVisits");

            migrationBuilder.DropIndex(
                name: "IX_BookedVisits_WorkerAvailabilityId",
                table: "BookedVisits");

            migrationBuilder.DropColumn(
                name: "WorkerAvailabilityId",
                table: "BookedVisits");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoPath",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
