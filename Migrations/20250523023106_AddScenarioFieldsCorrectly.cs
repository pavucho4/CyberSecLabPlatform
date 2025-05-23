using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CyberSecLabPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddScenarioFieldsCorrectly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Scenarios",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Scenarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                table: "Scenarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ScenarioResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentAttackAssignmentId = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    IsPassed = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DetailedLogJson = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioResults_StudentAttackAssignments_StudentAttackAssig~",
                        column: x => x.StudentAttackAssignmentId,
                        principalTable: "StudentAttackAssignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<int>(type: "integer", nullable: false),
                    StepText = table.Column<string>(type: "text", nullable: false),
                    IsFinal = table.Column<bool>(type: "boolean", nullable: false),
                    ScoreIfSuccess = table.Column<int>(type: "integer", nullable: false),
                    ScoreIfFail = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioSteps_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioStepOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioStepId = table.Column<int>(type: "integer", nullable: false),
                    OptionText = table.Column<string>(type: "text", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    NextStepId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioStepOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioStepOptions_ScenarioSteps_NextStepId",
                        column: x => x.NextStepId,
                        principalTable: "ScenarioSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScenarioStepOptions_ScenarioSteps_ScenarioStepId",
                        column: x => x.ScenarioStepId,
                        principalTable: "ScenarioSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttackSimulationStates_ScenarioId",
                table: "AttackSimulationStates",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioResults_StudentAttackAssignmentId",
                table: "ScenarioResults",
                column: "StudentAttackAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioStepOptions_NextStepId",
                table: "ScenarioStepOptions",
                column: "NextStepId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioStepOptions_ScenarioStepId",
                table: "ScenarioStepOptions",
                column: "ScenarioStepId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioSteps_ScenarioId",
                table: "ScenarioSteps",
                column: "ScenarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttackSimulationStates_Scenarios_ScenarioId",
                table: "AttackSimulationStates",
                column: "ScenarioId",
                principalTable: "Scenarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttackSimulationStates_Scenarios_ScenarioId",
                table: "AttackSimulationStates");

            migrationBuilder.DropTable(
                name: "ScenarioResults");

            migrationBuilder.DropTable(
                name: "ScenarioStepOptions");

            migrationBuilder.DropTable(
                name: "ScenarioSteps");

            migrationBuilder.DropIndex(
                name: "IX_AttackSimulationStates_ScenarioId",
                table: "AttackSimulationStates");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Scenarios");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Scenarios");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "Scenarios");
        }
    }
}
