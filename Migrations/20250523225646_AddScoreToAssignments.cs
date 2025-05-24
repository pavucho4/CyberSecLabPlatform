using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberSecLabPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreToAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentScore",
                table: "AttackSimulationStates",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentScore",
                table: "AttackSimulationStates");
        }
    }
}
