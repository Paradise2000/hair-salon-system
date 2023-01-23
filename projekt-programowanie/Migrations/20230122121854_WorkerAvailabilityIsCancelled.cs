using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projektprogramowanie.Migrations
{
    /// <inheritdoc />
    public partial class WorkerAvailabilityIsCancelled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isCancelled",
                table: "WorkersAvailabilities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCancelled",
                table: "WorkersAvailabilities");
        }
    }
}
