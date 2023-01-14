using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projektprogramowanie.Migrations
{
    /// <inheritdoc />
    public partial class TPHinUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedVisits_Clients_ClientId",
                table: "BookedVisits");

            migrationBuilder.DropForeignKey(
                name: "FK_BookedVisits_Workers_WorkerId",
                table: "BookedVisits");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkersAvailabilities_Workers_WorkerId",
                table: "WorkersAvailabilities");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedVisits_Users_ClientId",
                table: "BookedVisits",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedVisits_Users_WorkerId",
                table: "BookedVisits",
                column: "WorkerId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkersAvailabilities_Users_WorkerId",
                table: "WorkersAvailabilities",
                column: "WorkerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedVisits_Users_ClientId",
                table: "BookedVisits");

            migrationBuilder.DropForeignKey(
                name: "FK_BookedVisits_Users_WorkerId",
                table: "BookedVisits");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkersAvailabilities_Users_WorkerId",
                table: "WorkersAvailabilities");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "Id");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Clients_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Workers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BookedVisits_Clients_ClientId",
                table: "BookedVisits",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedVisits_Workers_WorkerId",
                table: "BookedVisits",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkersAvailabilities_Workers_WorkerId",
                table: "WorkersAvailabilities",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
