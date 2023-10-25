using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SequencePro.Application.Migrations
{
    /// <inheritdoc />
    public partial class AACompositionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AminoAcidComposition",
                table: "SequenceAnalyses");

            migrationBuilder.CreateTable(
                name: "AminoAcidComposition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SequenceAnalysisId = table.Column<Guid>(type: "uuid", nullable: false),
                    AminoAcid = table.Column<char>(type: "character(1)", nullable: false),
                    Proportion = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AminoAcidComposition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AminoAcidComposition_SequenceAnalyses_SequenceAnalysisId",
                        column: x => x.SequenceAnalysisId,
                        principalTable: "SequenceAnalyses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AminoAcidComposition_SequenceAnalysisId",
                table: "AminoAcidComposition",
                column: "SequenceAnalysisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AminoAcidComposition");

            migrationBuilder.AddColumn<string>(
                name: "AminoAcidComposition",
                table: "SequenceAnalyses",
                type: "json",
                nullable: false,
                defaultValue: "");
        }
    }
}
